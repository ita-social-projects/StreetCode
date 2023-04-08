using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Login;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Users
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
        public async Task<IActionResult> GetWithAdmin()
        {
            return Ok("text");
        }
    }
}
