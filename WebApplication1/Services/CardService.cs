using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Services
{
    public interface ICardService
    {
        Task<ServiceResult<IEnumerable<CardDTO>>> GetCardsByGroupAsync(int groupId, string user);
        Task<ServiceResult> CreateCardAsync(CardDTO cardDTO, string user);
        Task<ServiceResult> UpdateCardAsync(CardDTO cardDTO, string user);
        Task<ServiceResult> DeleteCardAsync(int id, string user);
    }

    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGroupUserService _groupUserService;

        public CardService(ApplicationDbContext db, IGroupUserService groupUserService)
        {
            _db = db;
            _groupUserService = groupUserService;
        }

        public async Task<ServiceResult<IEnumerable<CardDTO>>> GetCardsByGroupAsync(int groupId, string user)
        {
            var result = new ServiceResult<IEnumerable<CardDTO>>();
            try
            {
                //check if group exists
                if (await _db.Groups.FirstOrDefaultAsync(g => g.Id == groupId) == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Group not found";

                }
                else
                {

                    if(!_groupUserService.IsInGroup(user, groupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in group";
                        return result;
                    }
               
                    var cardsByGroup = await _db.Cards
                        .Where(g => g.GroupId == groupId)
                        .Select(c => new CardDTO()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Description = c.Description,
                            Status = c.Status,
                            Deadline = c.Deadline,
                            GroupId = c.GroupId,
                            Sprint = c.Sprint
                        })
                         .ToListAsync();

                    result.ErrorCode = ErrorCodeEnum.Success;
                    result.Result = cardsByGroup;

                }
            }
            catch (Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.NotFound;
                result.ErrorMessage = ex.Message;
            }
            return result;

        }

        public async Task<ServiceResult> CreateCardAsync(CardDTO cardDTO, string user)
        {

            var result = new ServiceResult();
            try
            {
                // Check if the card is null
                if(cardDTO == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Card is null";
                }            
                else
                {
                    if (!_groupUserService.IsInGroup(user, cardDTO.GroupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in group";
                        return result;
                    }

                    var card = new Card
                    {
                        Name = cardDTO.Name,
                        Description = cardDTO.Description,
                        Status = cardDTO.Status,
                        Deadline = cardDTO.Deadline,
                        GroupId = cardDTO.GroupId,
                        Sprint = cardDTO.Sprint
                    };

                    _db.Cards.Add(card);
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

        public async Task<ServiceResult> UpdateCardAsync(CardDTO cardDTO, string user)
        {
            var result = new ServiceResult();

            try
            {

                // Check if the card is null
                if (cardDTO == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Card is null";
                }
                else
                {
                    var excistingCard = await _db.Cards.FirstOrDefaultAsync(c => c.Id == cardDTO.Id);
                    if (excistingCard == null)
                    {
                        result.ErrorCode = ErrorCodeEnum.NotFound;
                        result.ErrorMessage = "Card not found";
                    }

                    if (!_groupUserService.IsInGroup(user, cardDTO.GroupId).Result)
                    {
                        result.ErrorCode = ErrorCodeEnum.BadRequest;
                        result.ErrorMessage = "User is not in group";
                        return result;
                    }


                    excistingCard.Name = cardDTO.Name;
                    excistingCard.Description = cardDTO.Description;
                    excistingCard.Status = cardDTO.Status;
                    excistingCard.Deadline = cardDTO.Deadline;
                    excistingCard.GroupId = cardDTO.GroupId;
                    excistingCard.Sprint = cardDTO.Sprint; 
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

        public async Task<ServiceResult> DeleteCardAsync(int cardId, string user)
        {
            var result = new ServiceResult();

            try
            {
                var card = await _db.Cards.SingleOrDefaultAsync(c => c.Id == cardId);
                if (card == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Card not found";
                }

                if (!_groupUserService.IsInGroup(user, card.GroupId).Result)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "User is not in group";
                    return result;
                }

                _db.Cards.Remove(card);
                await _db.SaveChangesAsync();
                result.ErrorCode = ErrorCodeEnum.Success;
            }
            catch(Exception ex)
            {
                result.ErrorCode = ErrorCodeEnum.InternalServerError;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
