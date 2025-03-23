using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Users.Delete;

public record DeleteUserCommand(string Email)
    : IRequest<Result<Unit>>;