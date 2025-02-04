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
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Streetcode.WebApi.Controllers.Authentication
{
    [ApiController]
    public class AuthController : BaseApiController
    {
        // Login action for regular user login
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        // Register action for new user registration
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RegisterResponseDTO))]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        // Refresh token action to obtain a new access token
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RefreshTokenResponceDTO))]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO token)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }

        // Logout action to invalidate the user session or token
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

            // Invalidate the authentication session (sign out)
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var result = await Mediator.Send(new LogoutCommand(userId));

            if (result.IsFailed)
            {
                return BadRequest(result.Errors.First().Message);
            }

            return Ok("Logout successful. Refresh token invalidated.");
        }

        // Google Login action to handle authentication via Google ID Token
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponseDTO))]
        public async Task<IActionResult> GoogleLogin([FromBody] string idToken)
        {
            var result = await Mediator.Send(new LoginGoogleQuery(idToken));

            if (result.IsSuccess)
            {
                var user = result.Value; // Assume this contains user data after successful login
                var roles = user.Roles; // Make sure roles are part of the returned user object

                // Add claims based on roles after successful Google login
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId),
                    new Claim(ClaimTypes.Name, user.Username)
                };

                // Add roles as claims
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "Google");
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(principal); // Sign in the user

                return Ok(result.Value);
            }

            return Unauthorized(new { message = result.Errors.FirstOrDefault()?.Message });
        }

        // Example of protected route for admin access only
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-dashboard")]
        public IActionResult AdminDashboard()
        {
            return Ok("Admin Dashboard");
        }

        // Example of profile action with access control for regular users and admins
        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var currentUser = User.Identity.Name;

            if (User.IsInRole("Admin"))
            {
                return NotFound(); // Redirect admin to 404 or similar (or redirect elsewhere if necessary)
            }

            // Fetch and return the user's profile info
            return Ok("User profile data");
        }
    }
}
