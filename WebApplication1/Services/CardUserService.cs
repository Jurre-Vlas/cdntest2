using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Services
{
    public interface ICardUserService
    {
        Task<ServiceResult> CreateCardUserAsync(int cardId, string userId, string user);
        Task<ServiceResult> RemoveCardUserAsync(int cardId, string userId, string user);
    }

    public class CardUserService : ICardUserService
    {
        private readonly ApplicationDbContext _db;
        private readonly IGroupUserService _groupUserService;

        public CardUserService(ApplicationDbContext db, IGroupUserService groupUserService)
        {
            _db = db;
            _groupUserService = groupUserService;
        }

        public async Task<ServiceResult> CreateCardUserAsync(int cardId, string userId, string user)
        {
            var result = new ServiceResult();

            try
            {
                var card = await _db.Cards.FindAsync(cardId);
                if (card == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Card not found";
                    return result;
                }

                var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);
                if (authenticatedUserCheck == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Authenticated User not found";
                    return result;
                }

                if (!_groupUserService.IsInGroup(user, card.GroupId).Result)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "User is not in group";
                    return result;
                }

                var userToAdd = await _db.Users.FindAsync(userId);
                if (userToAdd == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "User to add not found";
                    return result;
                }

                var existingCardUser = await _db.CardUsers.FirstOrDefaultAsync(cu => cu.CardId == cardId && cu.UserId == userId);
                if (existingCardUser != null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Card user already exists";
                    return result;
                }

                _db.CardUsers.Add(new CardUser { CardId = cardId, UserId = userId });
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

        public async Task<ServiceResult> RemoveCardUserAsync(int cardId, string userId, string user)
        {
            var result = new ServiceResult();

            try
            {
                var card = await _db.Cards.FindAsync(cardId);
                if (card == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Card not found";
                    return result;
                }

                var authenticatedUserCheck = await _db.Users.SingleOrDefaultAsync(u => u.UserName == user);
                if (authenticatedUserCheck == null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Authenticated User not found";
                    return result;
                }

                if (!_groupUserService.IsInGroup(user, card.GroupId).Result)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "User is not in group";
                    return result;
                }

                var cardUser = await _db.CardUsers.FirstOrDefaultAsync(cu => cu.CardId == cardId && cu.UserId == userId);
                if (cardUser == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "Card user not found";
                    return result;
                }

                _db.CardUsers.Remove(cardUser);
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
