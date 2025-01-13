using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Transactions.TransactionLink.GetById;
using System.Security.Claims;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.MediatR.Users.Delete;
using Streetcode.BLL.MediatR.Users.GetByName;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.MediatR.Users.GetByUserName;

namespace Streetcode.WebApi.Controllers.Users
{
    public class UsersController : BaseApiController
    {
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUserId()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            return HandleResult(await Mediator.Send(new GetByUserIdQuery(userId)));
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
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            return HandleResult(await Mediator.Send(new UpdateUserCommand(user)));
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(DeleteUserDTO deleteUserDto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User is not authenticated.");
            }

            deleteUserDto.UserId = userId;

            return HandleResult(await Mediator.Send(new DeleteUserCommand(deleteUserDto)));
        }
    }
}
