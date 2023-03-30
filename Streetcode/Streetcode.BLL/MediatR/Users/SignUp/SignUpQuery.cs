using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.MediatR.Users.SignUp
{
    public record SignUpQuery(UserDTO newUser) : IRequest<Result<UserDTO>>;
}
