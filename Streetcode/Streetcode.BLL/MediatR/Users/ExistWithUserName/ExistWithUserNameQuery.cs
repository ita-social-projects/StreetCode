using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Users.ExistWithUserName;

public record ExistWithUserNameQuery(string UserName)
    : IRequest<Result<bool>>;