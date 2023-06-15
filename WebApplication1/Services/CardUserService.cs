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
        Task<ServiceResult> CreateCardUserAsync(int cardId, string NameUser, string user);
        Task<ServiceResult> RemoveCardUserAsync(int cardId, string NameUser, string user);
        Task<ServiceResult<IEnumerable<CardUserDTO>>> GetAllUsersFromCardIdAsync(int cardId, string user);
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

        public async Task<ServiceResult> CreateCardUserAsync(int cardId, string NameUser, string user)
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

                var userToAdd = await _db.Users.SingleOrDefaultAsync(c => c.Name ==NameUser );
                if (userToAdd == null)
                {
                    result.ErrorCode = ErrorCodeEnum.NotFound;
                    result.ErrorMessage = "User to add not found";
                    return result;
                }

                var existingCardUser = await _db.CardUsers.FirstOrDefaultAsync(cu => cu.CardId == cardId && cu.UserId == userToAdd.Id);
                if (existingCardUser != null)
                {
                    result.ErrorCode = ErrorCodeEnum.BadRequest;
                    result.ErrorMessage = "Card user already exists";
                    return result;
                }

                _db.CardUsers.Add(new CardUser { CardId = cardId, UserId = userToAdd.Id });
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

        public async Task<ServiceResult> RemoveCardUserAsync(int cardId, string NameUser, string user)
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

                var userCard = await _db.Users.SingleOrDefaultAsync(c => c.Name == NameUser);

                var cardUser = await _db.CardUsers.FirstOrDefaultAsync(cu => cu.CardId == cardId && cu.UserId == userCard.Id);
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

        public async Task<ServiceResult<IEnumerable<CardUserDTO>>> GetAllUsersFromCardIdAsync(int cardId, string user)
        {
            var result = new ServiceResult<IEnumerable<CardUserDTO>>();

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


                var cardUsers = await _db.CardUsers
                    .Where(cu => cu.CardId == cardId)
                    .Select(cu => new CardUserDTO
                    {
                        UserId = cu.UserId,
                        CardId = cu.CardId
                    })
                    .ToListAsync();

                result.Result = cardUsers;

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
