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
    public interface ICardTaskService
    {
        Task<ServiceResult<IEnumerable<CardTaskDTO>>> GetTasksByCardAsync(int cardId, string user);
        Task<ServiceResult> CreateTaskAsync(CardTaskDTO taskDTO, string user);
        Task<ServiceResult> UpdateTaskAsync(CardTaskDTO taskDTO, string user);
        Task<ServiceResult> DeleteTaskAsync(int taskId, string user);
    }

    public class CardTaskService : ICardTaskService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGroupUserService _groupUserService;

        public CardTaskService(ApplicationDbContext db, IGroupUserService groupUserService)
        {
            _db = db;
            _groupUserService = groupUserService;
        }

        public async Task<ServiceResult<IEnumerable<CardTaskDTO>>> GetTasksByCardAsync(int cardId, string user)
        {
            var result = new ServiceResult<IEnumerable<CardTaskDTO>>();
            try
            {
                // Check if the card exists
                if (await _db.Cards.FirstOrDefaultAsync(c => c.Id == cardId) == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Card not found";
                }
                else
                {
                    // Check if the user is in the group of the card
                    var groupId = await _db.Cards.Where(c => c.Id == cardId).Select(c => c.GroupId).FirstOrDefaultAsync();
                    if (!_groupUserService.IsInGroup(user, groupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in the group";
                        return result;
                    }

                    var tasksByCard = await _db.CardTasks
                        .Where(t => t.CardId == cardId)
                        .Select(t => new CardTaskDTO()
                        {
                            Id = t.Id,
                            Name = t.Name,
                            Description = t.Description,
                            Status = t.Status,
                            CardId = t.CardId
                        })
                        .ToListAsync();

                    result.ErrorCode = ErrorCodeEnum.Success;
                    result.Result = tasksByCard;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public async Task<ServiceResult> CreateTaskAsync(CardTaskDTO taskDTO, string user)
        {
            var result = new ServiceResult();
            try
            {
                // Check if the task is null
                if (taskDTO == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Task is null";
                }
                else
                {
                    // Check if the card exists
                    if (await _db.Cards.FirstOrDefaultAsync(c => c.Id == taskDTO.CardId) == null)
                    {
                        result.ErrorCode = ErrorCodeEnum.NotFound;
                        result.ErrorMessage = "Card not found";
                        return result;
                    }

                    // Check if the user is in the group of the card
                    var groupId = await _db.Cards.Where(c => c.Id == taskDTO.CardId).Select(c => c.GroupId).FirstOrDefaultAsync();
                    if (!_groupUserService.IsInGroup(user, groupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in the group";
                        return result;
                    }

                    var task = new CardTask
                    {
                        Name = taskDTO.Name,
                        Description = taskDTO.Description,
                        Status = taskDTO.Status,
                        CardId = taskDTO.CardId
                    };

                    _db.CardTasks.Add(task);
                    await _db.SaveChangesAsync();

                    result.ErrorCode = ErrorCodeEnum.Success;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<ServiceResult> UpdateTaskAsync(CardTaskDTO taskDTO, string user)
        {
            var result = new ServiceResult();

            try
            {
                // Check if the task is null
                if (taskDTO == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Task is null";
                }
                else
                {
                    var existingTask = await _db.CardTasks.FirstOrDefaultAsync(t => t.Id == taskDTO.Id);
                    if (existingTask == null)
                    {
                        result.ErrorCode = ErrorCodeEnum.NotFound;
                        result.ErrorMessage = "Task not found";
                    }

                    // Check if the user is in the group of the card
                    var groupId = await _db.Cards.Where(c => c.Id == taskDTO.CardId).Select(c => c.GroupId).FirstOrDefaultAsync();
                    if (!_groupUserService.IsInGroup(user, groupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in the group";
                        return result;
                    }

                    existingTask.Name = taskDTO.Name;
                    existingTask.Description = taskDTO.Description;
                    existingTask.Status = taskDTO.Status;
                    existingTask.CardId = taskDTO.CardId;
                    await _db.SaveChangesAsync();

                    result.ErrorCode = ErrorCodeEnum.Success;
                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public async Task<ServiceResult> DeleteTaskAsync(int taskId, string user)
        {
            var result = new ServiceResult();

            try
            {
                var task = await _db.CardTasks.SingleOrDefaultAsync(t => t.Id == taskId);
                if (task == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Task not found";
                }

                // Check if the user is in the group of the card
                var groupId = await _db.Cards.Where(c => c.Id == task.CardId).Select(c => c.GroupId).FirstOrDefaultAsync();
                if (!_groupUserService.IsInGroup(user, groupId).Result)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "User is not in the group";
                    return result;
                }

                _db.CardTasks.Remove(task);
                await _db.SaveChangesAsync();
                result.ErrorCode = ErrorCodeEnum.Success;
            }
            catch (Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}
