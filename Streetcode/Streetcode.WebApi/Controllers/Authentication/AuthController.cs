using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Logging;
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
    [EnableRateLimiting("api")] // Apply rate limiting to all endpoints
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            _logger.LogInformation("Login attempt for user: {Email}", loginDTO.Email);
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDTO))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            _logger.LogInformation("New user registration attempt: {Email}", registerDTO.Email);
            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponceDTO))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO token)
        {
            _logger.LogInformation("Refresh token attempt.");
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }

        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Logout()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Unauthorized logout attempt.");
                return Unauthorized("User is not authenticated.");
            }

            var result = await Mediator.Send(new LogoutCommand(userId));

            if (result.IsFailed)
            {
                _logger.LogError("Logout failed for user: {UserId}", userId);
                return BadRequest(result.Errors.First().Message);
            }

            _logger.LogInformation("User {UserId} logged out successfully.", userId);
            return Ok("Logout successful. Refresh token invalidated.");
        }

        [HttpPost("google-login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            _logger.LogInformation("Google login attempt.");
            var result = await Mediator.Send(new LoginGoogleQuery(idToken));

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            _logger.LogWarning("Google login failed.");
            return Unauthorized(new { message = result.Errors.FirstOrDefault()?.Message });
        }
    }
}
