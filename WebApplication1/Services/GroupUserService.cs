using Azure.Core;
using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Microsoft.EntityFrameworkCore;

using System;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Services
{
    public interface IGroupUserService
    {
        Task<ServiceResult> AddUserToGroupAsync(string authenticatedUser, string addedUser, int groupId);
        Task<ServiceResult> RemoveUserFromGroupAsync(string authenticatedUser, string removingUser, int groupId);
        Task<ServiceResult> LeaveGroup(string authenticatedUser, int groupId);
        Task<bool> IsInGroup(string user, int groupId);
        }

    public class GroupUserService : IGroupUserService
    {
        private readonly ApplicationDbContext _db;

        public GroupUserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<ServiceResult> AddUserToGroupAsync(string authenticatedUser, string addedUser, int groupId)
        {
            var result = new ServiceResult();
            try
            {
                // Check if the User exists
                var user = await _db.Users.SingleOrDefaultAsync(u => u.Name == addedUser);
                if (user == null)
                {
                    result.ErrorMessage = "User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }



                // Check if the Group exists
                var group = await _db.Groups.FirstOrDefaultAsync(u => u.Id == groupId);
                if (group == null)
                {
                    result.ErrorMessage = "Group not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;

                }


                // Check if the User is already in the Group
                var existingGroupUser = await _db.GroupUsers.AnyAsync(u => u.UserId == user.Id && u.GroupId == groupId);
                if (existingGroupUser)
                {
                    result.ErrorMessage = "User already belongs to the Group";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if the User is the authenticated user
                var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == authenticatedUser);
                if(authenticatedUserCheck == null)
                {
                    result.ErrorMessage = "Authenticated User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if the authenticated user is an admin of the group
                var authenticatedUserIsAdmin = await _db.GroupUsers.AnyAsync(u => u.UserId == authenticatedUserCheck.Id && u.GroupId == groupId && u.Role == "GroupAdmin");
                if (authenticatedUserIsAdmin)
                {
                    var newGroupUser = new GroupUser
                    {
                        UserId = user.Id,
                        GroupId = groupId,
                        Role = "User"
                    };

                    _db.GroupUsers.Add(newGroupUser);
                    await _db.SaveChangesAsync();
                } else
                {
                    result.ErrorMessage = "User is not an group Admin";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }
            return result;
        }


        public async Task<ServiceResult> RemoveUserFromGroupAsync(string authenticatedUser, string removingUser, int groupId)
        {
            var result = new ServiceResult();
            try
            {

                // Check if the User exists
                var user = await _db.Users.SingleOrDefaultAsync(u => u.Name == removingUser);
                if (user == null)
                {
                    result.ErrorMessage = "User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                // Check if the Group exists
                var group = await _db.Groups.FirstOrDefaultAsync(u => u.Id == groupId);
                if (group == null)
                {
                    result.ErrorMessage = "Group not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;

                }

                // Check if the User is  in the Group
                var existingGroupUser = await _db.GroupUsers.FirstOrDefaultAsync(u => u.UserId == user.Id && u.GroupId == groupId);
                if (existingGroupUser == null)
                {
                    result.ErrorMessage = "User is not in the Group";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if the User is the authenticated user
                var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == authenticatedUser);
                if (authenticatedUserCheck == null)
                {
                    result.ErrorMessage = "Authenticated User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if the authenticated user is an admin of the group
                var authenticatedUserIsAdmin = await _db.GroupUsers.AnyAsync(u => u.UserId == authenticatedUserCheck.Id && u.GroupId == groupId && u.Role == "GroupAdmin");
                if (authenticatedUserIsAdmin)
                {
                    _db.GroupUsers.Remove(existingGroupUser);
                    await _db.SaveChangesAsync();
                }
                else
                {
                    result.ErrorMessage = "User is not an group Admin";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }

        public async Task<ServiceResult> LeaveGroup(string authenticatedUser, int groupId)
        {
            var result = new ServiceResult();
            try
            {

                // Check if the User exists
                var user = await _db.Users.SingleOrDefaultAsync(u => u.UserName == authenticatedUser);
                if (user == null)
                {
                    result.ErrorMessage = "User not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                // Check if the Group exists
                var group = await _db.Groups.FirstOrDefaultAsync(u => u.Id == groupId);
                if (group == null)
                {
                    result.ErrorMessage = "Group not found";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;

                }

                // Check if the User is  in the Group
                var existingGroupUser = await _db.GroupUsers.FirstOrDefaultAsync(u => u.UserId == user.Id && u.GroupId == groupId);
                if (existingGroupUser == null)
                {
                    result.ErrorMessage = "User is not in the Group";
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    return result;
                }

                //Check if the authenticated user is an admin of the group
                if (existingGroupUser.Role == "GroupAdmin")
                {
                    var otherAdmins = await _db.GroupUsers.AnyAsync(u => u.GroupId == groupId && u.Role == "GroupAdmin" && u.UserId != existingGroupUser.UserId);
                    if (!otherAdmins)
                    {
                        result.ErrorMessage = "You are the last admin in the Group. You cannot leave the Group.";
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        return result;
                    }
                }

                _db.GroupUsers.Remove(existingGroupUser);
                await _db.SaveChangesAsync();

                result.ErrorCode = ErrorCodeEnum.Success;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
            }

            return result;
        }

        public async Task<bool> IsInGroup(string user, int groupId)
        {
            // Check if user exists
            var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);
            if (authenticatedUserCheck == null)
            {
                return false;
            }

            // Check if user is in group
            var groupUserCheck = await _db.GroupUsers.AnyAsync(u => u.UserId == authenticatedUserCheck.Id && u.GroupId == groupId);

            if (groupUserCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
