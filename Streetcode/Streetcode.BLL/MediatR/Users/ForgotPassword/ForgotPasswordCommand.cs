using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users.Password;

namespace Streetcode.BLL.MediatR.Users.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordDTO ForgotPasswordDto) : IRequest<Result<Unit>>
{

}