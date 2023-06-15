
using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Models.DTO;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;


        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;

        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServiceResult<IEnumerable<GroupDTO>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<ActionResult<ServiceResult<IEnumerable<GroupDTO>>>> GetGroupsAsync()
        {
            var user = User.FindFirstValue(ClaimTypes.Name);

            var serviceResult = await _groupService.GetGroupsByUserAsync(user);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok(serviceResult);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
       

        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> CreateGroup([FromBody] GroupDTO groupDTO)
        {
            //find authenticated user name
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupService.CreateGroupAsync(groupDTO, user);
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
        public async Task<IActionResult> UpdateGroup(int id, [FromBody] GroupDTO groupDTO)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupService.UpdateGroupAsync(user ,groupDTO);
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

        [HttpDelete("{groupId:int}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var user = User.FindFirstValue(ClaimTypes.Name);
            var serviceResult = await _groupService.DeleteGroupAsync(groupId, user);
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
