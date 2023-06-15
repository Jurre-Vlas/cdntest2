using CampaingControlCenterAPI.Services;
using Eindopdrachtcnd2.Data.Entities;
using Eindopdrachtcnd2.Models;
using Eindopdrachtcnd2.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eindopdrachtcnd2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }


        [HttpPost]
        [Route("Register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status403Forbidden, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterUserDTO registerUserDTO)
        {
            var serviceResult = await _authenticationService.RegisterAsync(registerUserDTO);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                case ErrorCodeEnum.InternalServerError:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        [HttpPost]
        [Route("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var serviceResult = await _authenticationService.LoginAsync(loginDTO);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok(serviceResult);
                case ErrorCodeEnum.Unauthorized:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status401Unauthorized);
                case ErrorCodeEnum.BadRequest:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status400BadRequest);
                case ErrorCodeEnum.InternalServerError:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
        {
            var serviceResult = await _authenticationService.UpdateUserRole(userId, newRole);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok();
                case ErrorCodeEnum.NotFound:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status404NotFound);
                case ErrorCodeEnum.InternalServerError:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("checkToken")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
        public async Task<IActionResult> IsTokenValidAsync(string token)
        {
            var serviceResult = await _authenticationService.IsTokenValidAsync(token);
            switch (serviceResult.ErrorCode)
            {
                case ErrorCodeEnum.Success:
                    return Ok(serviceResult);
                case ErrorCodeEnum.InternalServerError:
                    return Problem(serviceResult.ErrorMessage, statusCode: StatusCodes.Status500InternalServerError);
                default:
                    return Problem("Server Error", statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}
