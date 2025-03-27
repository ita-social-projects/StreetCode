using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.ConfirmEmail;

namespace Streetcode.BLL.MediatR.Authentication.ConfirmEmail;

public record ConfirmEmailCommand(ConfirmEmailDto ConfirmEmailDto) : IRequest<Result<bool>>;