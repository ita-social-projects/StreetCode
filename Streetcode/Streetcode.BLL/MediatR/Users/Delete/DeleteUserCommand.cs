using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.Delete;

public record DeleteUserCommand(string Email) : IRequest<Result<Unit>>
{

}
