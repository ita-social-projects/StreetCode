using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.Login;

namespace Streetcode.BLL.MediatR.Authentication.LoginGoogle;

public record LoginGoogleQuery(string idToken)
    : IRequest<Result<LoginResponseDto>>;