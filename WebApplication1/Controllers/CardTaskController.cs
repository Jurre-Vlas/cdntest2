using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardTaskController : ControllerBase
    {
        private readonly ICardTaskService _cardTaskService;

        public CardTaskController(ICardTaskService cardTaskService)
        {
            _cardTaskService = cardTaskService;
        }

        [HttpGet("{cardId:int}/tasks")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResult<IEnumerable<CardTaskDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ServiceResult<IEnumerable<CardTaskDTO>>>> GetTasksByCardAsync(int cardId)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _cardTaskService.GetTasksByCardAsync(cardId, user);

            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok(serviceResult);
                case ErrorCodeEnum.NotFound:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound);
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("{cardId:int}/tasks")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateTaskAsync(int cardId, [FromBody] CardTaskDTO taskDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            taskDTO.CardId = cardId;

            var serviceResult = await _cardTaskService.CreateTaskAsync(taskDTO, user);

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

        [HttpPut("{cardId:int}/tasks/{taskId:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateTaskAsync(int cardId, int taskId, [FromBody] CardTaskDTO taskDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            taskDTO.CardId = cardId;
            taskDTO.Id = taskId;

            var serviceResult = await _cardTaskService.UpdateTaskAsync(taskDTO, user);

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

        [HttpDelete("{cardId:int}/tasks/{taskId:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteTaskAsync(int cardId, int taskId)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            var serviceResult = await _cardTaskService.DeleteTaskAsync(taskId, user);

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
