using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public record RegisterQuery(UserRegisterDTO newUser) : IRequest<Result<UserDTO>>;
}
