using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Authentication.ConfirmEmail;

namespace Streetcode.BLL.MediatR.Authentication.ConfirmEmail;

public record ConfirmEmailCommand(ConfirmEmailDTO ConfirmEmailDto) : IRequest<Result<bool>>;