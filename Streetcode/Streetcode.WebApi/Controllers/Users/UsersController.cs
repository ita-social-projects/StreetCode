using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Password;
using Streetcode.BLL.MediatR.Users.Delete;
using Streetcode.BLL.MediatR.Users.ForgotPassword;
using Streetcode.BLL.MediatR.Users.GetByName;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.MediatR.Users.GetByUserName;
using Streetcode.BLL.MediatR.Users.UpdateForgotPassword;

namespace Streetcode.WebApi.Controllers.Users
{
    public class UsersController : BaseApiController
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId()
        {
            return HandleResult(await Mediator.Send(new GetByUserIdQuery()));
        }

        [HttpGet("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserName([FromRoute] string userName)
        {
            return HandleResult(await Mediator.Send(new GetByUserNameQuery(userName)));
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(UpdateUserDTO user)
        {
            return HandleResult(await Mediator.Send(new UpdateUserCommand(user)));
        }

        [HttpDelete("{email}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([FromRoute] string email)
        {
            return HandleResult(await Mediator.Send(new DeleteUserCommand(email)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO forgotPasswordDto)
        {
            return HandleResult(await Mediator.Send(new ForgotPasswordCommand(forgotPasswordDto)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateForgotPassword(UpdateForgotPasswordDTO updateForgotPasswordDto)
        {
            return HandleResult(await Mediator.Send(new UpdateForgotPasswordCommand(updateForgotPasswordDto)));
        }
    }
}
