using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Email;

namespace Streetcode.BLL.MediatR.Email;
public record SendEmailCommand(EmailDTO Email) : IRequest<Result<Unit>>;
