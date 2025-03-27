using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Authentication.ConfirmEmail;
using Streetcode.BLL.DTO.Authentication.GoogleLogin;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Authentication.RefreshToken;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.DTO.Authentication.ValidateToken;
using Streetcode.BLL.MediatR.Authentication.ConfirmEmail;
using Streetcode.BLL.MediatR.Authentication.Login;
using Streetcode.BLL.MediatR.Authentication.LoginGoogle;
using Streetcode.BLL.MediatR.Authentication.Logout;
using Streetcode.BLL.MediatR.Authentication.RefreshToken;
using Streetcode.BLL.MediatR.Authentication.Register;
using Streetcode.BLL.MediatR.Authentication.ValidateToken;

namespace Streetcode.WebApi.Controllers.Authentication
{
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDTO))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponceDTO))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO token)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var result = await Mediator.Send(new LogoutCommand());

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message);
            }

            return Ok("Logout successful. Refresh token invalidated.");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> GoogleLogin(GoogleLoginRequest googleLoginRequest)
        {
            var result = await Mediator.Send(new LoginGoogleQuery(googleLoginRequest.IdToken));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return Unauthorized(new { message = result.Errors.FirstOrDefault()?.Message });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO confirmEmailDto)
        {
            return HandleResult(await Mediator.Send(new ConfirmEmailCommand(confirmEmailDto)));
        }

        [HttpPost]
        public async Task<IActionResult> ValidateToken(ValidateTokenDto confirmEmailDto)
        {
            return HandleResult(await Mediator.Send(new ValidateTokenCommand(confirmEmailDto)));
        }
    }
}
