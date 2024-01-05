using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public record SignUpQuery(UserRegisterDTO newUser) : IRequest<Result<UserDTO>>;
}
