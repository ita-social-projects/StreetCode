using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Users.GetAllUserName;

public record ExistWithUserNameQuery(string UserName) : IRequest<Result<bool>>;