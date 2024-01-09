using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.Login;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.Login
{
    public record LoginQuery(LoginRequestDTO UserLogin) : IRequest<Result<LoginResponseDTO>>;
}
