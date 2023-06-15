using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet("{groupId:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResult<IEnumerable<CardDTO>>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ServiceResult<IEnumerable<CardDTO>>>> GetCardsByGroupAsync(int groupId)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _cardService.GetCardsByGroupAsync(groupId, user);

            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok(serviceResult);
                case ErrorCodeEnum.NotFound:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateCard([FromBody] CardDTO cardDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _cardService.CreateCardAsync(cardDTO, user);

            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                default:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateCard(int id, [FromBody] CardDTO cardDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _cardService.UpdateCardAsync(cardDTO, user);

            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                case ErrorCodeEnum.NotFound:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteCard(int id)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _cardService.DeleteCardAsync(id, user);

            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.NotFound:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
