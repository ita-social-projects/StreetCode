using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users.Password;

namespace Streetcode.BLL.MediatR.Users.UpdateForgotPassword;

public record UpdateForgotPasswordCommand(UpdateForgotPasswordDTO UpdateForgotPasswordDto) : IRequest<Result<Unit>>
{
}