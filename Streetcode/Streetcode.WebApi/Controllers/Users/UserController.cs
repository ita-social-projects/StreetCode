using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Login;
using Streetcode.BLL.MediatR.Users.RefreshToken;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Users
{
    public class UserController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO loginDTO)
        {
            return HandleResult(await Mediator.Send(new LoginQuery(loginDTO)));
        }

        [HttpPost]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO token)
        {
            return HandleResult(await Mediator.Send(new RefreshTokenQuery(token)));
        }
    }
}
