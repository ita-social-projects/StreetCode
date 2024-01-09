using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.Login
{
    public record LoginQuery(UserLoginDTO UserLogin) : IRequest<Result<LoginResultDTO>>;
}
