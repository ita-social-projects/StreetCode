using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.Login;

namespace Streetcode.BLL.MediatR.Authentication.Login
{
    public record LoginQuery(LoginRequestDTO UserLogin)
        : IRequest<Result<LoginResponseDTO>>;
}
