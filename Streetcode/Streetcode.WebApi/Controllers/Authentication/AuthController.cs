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
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    [ApiController]
    [EnableRateLimiting("api")] // General rate limiting for all endpoints
    [Authorize] // Require authentication by default
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous] // Explicitly allow unauthenticated users
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            _logger.LogInformation("Login attempt received.");
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [EnableRateLimiting("registration")] // Stricter rate limiting for registration
        [ValidateAntiForgeryToken] // CSRF protection
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDTO))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            _logger.LogInformation("New user registration attempt received."); // No PII logging

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [EnableRateLimiting("token-refresh")] // Stricter rate limiting for token refresh
        [ValidateAntiForgeryToken] // CSRF protection
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponceDTO))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO token)
        {
            _logger.LogInformation("Refresh token attempt.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

        [AllowAnonymous]
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

        // Admin-only endpoint to retrieve users
        [Authorize(Roles = Roles.Admin)]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            _logger.LogInformation("Admin accessing user list.");
            // Implementation of user retrieval logic here
            return Ok(new { message = "User list retrieved successfully." });
        }
    }
}
