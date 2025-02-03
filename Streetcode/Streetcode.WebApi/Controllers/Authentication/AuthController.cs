using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.BLL.MediatR.Authentication.LoginGoogle;
using Streetcode.BLL.MediatR.Authentication.Logout;
using Streetcode.BLL.MediatR.Authentication.RefreshToken;
using Streetcode.BLL.MediatR.Authentication.Register;

namespace Streetcode.WebApi.Controllers.Authentication
{
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDto))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerDTO)
        {
            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponceDto))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto token)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            var result = await Mediator.Send(new LogoutCommand(userId));

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message);
            }

            return Ok("Logout successful. Refresh token invalidated.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDto))]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var result = await Mediator.Send(new LoginGoogleQuery(idToken));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Unauthorized(new { message = result.Errors.FirstOrDefault()?.Message });
        }
    }
}
