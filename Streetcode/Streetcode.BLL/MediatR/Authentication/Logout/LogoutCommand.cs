using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Authentication.Logout;

public record LogoutCommand : IRequest<Result>;
