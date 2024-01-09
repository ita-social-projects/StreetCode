using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Authentication;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Login;
using Streetcode.BLL.MediatR.Users.RefreshToken;
using Streetcode.BLL.MediatR.Users.SignUp;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Users
{
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthController([FromServices] UserManager<User> userManager, [FromServices] RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerDTO)
        {
            return HandleResult(await Mediator.Send(new RegisterQuery(registerDTO)));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO token)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }
    }
}
