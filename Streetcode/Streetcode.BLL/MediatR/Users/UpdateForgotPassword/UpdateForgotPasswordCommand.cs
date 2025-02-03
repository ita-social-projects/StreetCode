using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Users.Password;

namespace Streetcode.BLL.MediatR.Users.UpdateForgotPassword;

public record UpdateForgotPasswordCommand(UpdateForgotPasswordDto UpdateForgotPasswordDto) : IRequest<Result<Unit>>;