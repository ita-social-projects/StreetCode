using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.RefreshToken
{
    public record RefreshTokenQuery(RefreshTokenDTO token) : IRequest<Result<RefreshTokenResponce>>;
}
