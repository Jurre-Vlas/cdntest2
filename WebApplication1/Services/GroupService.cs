using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Services
{
    public interface IGroupService
    {
        //Task<ServiceResult<IEnumerable<GroupDTO>>> GetGroupsAsync();
        Task<ServiceResult<List<GroupDTO>>> GetGroupsByUserAsync(string user);
        Task<ServiceResult> CreateGroupAsync(GroupDTO groupDTO, string userName);
        Task<ServiceResult> UpdateGroupAsync(string user, GroupDTO groupDTO);
        Task<ServiceResult> DeleteGroupAsync(int groupId, string user);
    }

    public class GroupService : IGroupService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGroupUserService _groupUserService;
        private readonly ILogger<GroupService> _logger;


        public GroupService(ApplicationDbContext db, IGroupUserService groupUserService, ILogger<GroupService> logger)
        {
            _db = db;
            _groupUserService = groupUserService;
            _logger = logger;
        }

        //public async Task<ServiceResult<IEnumerable<GroupDTO>>> GetGroupsAsync()
        //{
        //    var result = new ServiceResult<IEnumerable<GroupDTO>>();

        //    try
        //    {
        //        var groups = await _db.Groups
        //            .Select(g => new GroupDTO { Id = g.Id, Name = g.Name, Description = g.Description })
        //            .ToListAsync();

        //        result.Result = groups;
        //        result.ErrorCode = ErrorCodeEnum.Success;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMessage = ex.Message;
        //        result.ErrorCode = ErrorCodeEnum.InternalServerError;
        //    }

        //    return result;
        //}

        public async Task<ServiceResult<List<GroupDTO>>> GetGroupsByUserAsync(string user)
        {
            var result = new ServiceResult<List<GroupDTO>>();

            try
            {
                var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);

                var userGroups = await _db.GroupUsers
                    .Where(g => g.UserId == authenticatedUserCheck.Id)
                    .Include(g => g.Group) // Laad de gerelateerde Group-entiteit
                    .Select(g => new GroupDTO { Id = g.Group.Id, Name = g.Group.Name, Description = g.Group.Description })
                    .ToListAsync();

                result.Result = userGroups;
                result.ErrorCode = ErrorCodeEnum.Success;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }

        public async Task<ServiceResult> CreateGroupAsync(GroupDTO groupDTO, string userName)
        {
            var result = new ServiceResult();

            try
            {
                // Controleer of de gebruikersnaam bestaat in de AspNetUsers-tabel
                var user = await _db.Users.SingleOrDefaultAsync(u => u.UserName == userName);
                if (user == null)
                {
                    result.ErrorMessage = "User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                if (await _db.Groups.AnyAsync(g => g.Name.ToLower() == groupDTO.Name.ToLower()))
                {
                    result.ErrorMessage = "Group already exists";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                }
                else
                {
                    var group = new Group { Name = groupDTO.Name, Description = groupDTO.Description };
                    _db.Groups.Add(group);
                    await _db.SaveChangesAsync();

                    int createdGroupUserId = group.Id;
                    _logger.LogInformation($"Group created with id {createdGroupUserId} by user {user.Id}");

                    _db.GroupUsers.Add(new GroupUser { UserId = user.Id, GroupId = createdGroupUserId, Role = "GroupAdmin" });
                    await _db.SaveChangesAsync();
                    result.ErrorCode = ErrorCodeEnum.Success;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }


        public async Task<ServiceResult> UpdateGroupAsync(string user, GroupDTO groupDTO)
        {
            var result = new ServiceResult();

            try
            {
                var group = await _db.Groups.FindAsync(groupDTO.Id);

                if (group == null)
                {
                    result.ErrorMessage = "Group not found";
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                }
                else
                {
                    var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);
                    if (authenticatedUserCheck == null)
                    {
                        result.ErrorMessage = "Authenticated User not found";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        return result;
                    }

                    var authenticatedUserIsAdmin = await _db.GroupUsers.AnyAsync(u => u.UserId == authenticatedUserCheck.Id && u.GroupId == groupDTO.Id && u.Role == "GroupAdmin");

                    if (authenticatedUserIsAdmin)
                    {
                        group.Name = groupDTO.Name;
                        group.Description = groupDTO.Description;

                        await _db.SaveChangesAsync();

                        result.ErrorCode = ErrorCodeEnum.Success;
                    }
                    else
                    {
                        result.ErrorMessage = "Authenticated User is not an admin of this group";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }

        public async Task<ServiceResult> DeleteGroupAsync(int groupId, string user)
        {
            var result = new ServiceResult();

            try
            {
                var group = await _db.Groups.FindAsync(groupId);

                if (group == null)
                {
                    result.ErrorMessage = "Group not found";
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                }
                else
                {
                    var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);
                    if (authenticatedUserCheck == null)
                    {
                        result.ErrorMessage = "Authenticated User not found";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        return result;
                    }

                    var authenticatedUserIsAdmin = await _db.GroupUsers.AnyAsync(u => u.UserId == authenticatedUserCheck.Id && u.GroupId == group.Id && u.Role == "GroupAdmin");

                    if (authenticatedUserIsAdmin)
                    {
                        _db.Groups.Remove(group);
                        await _db.SaveChangesAsync();

                        result.ErrorCode = ErrorCodeEnum.Success;
                    }
                    else
                    {
                        result.ErrorMessage = "Authenticated User is not an admin of this group";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }
    }
}
