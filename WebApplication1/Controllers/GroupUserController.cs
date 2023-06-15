using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
        private readonly IGroupUserService _groupUserService;

        public GroupUserController(IGroupUserService groupUserService)
        {
            _groupUserService = groupUserService;
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> AddUserToGroupAsync(string addingName, int groupId)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupUserService.AddUserToGroupAsync(user, addingName, groupId);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("remove")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> RemoveUserFromGroupAsync(string removeName, int groupId)
        {
            var authenticatedUser = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupUserService.RemoveUserFromGroupAsync(authenticatedUser, removeName, groupId);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost("leave")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> LeaveGroupAsync( int groupId)
        {
            var authenticatedUser = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupUserService.LeaveGroup(authenticatedUser, groupId);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
