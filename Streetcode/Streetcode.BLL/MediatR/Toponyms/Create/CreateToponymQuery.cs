using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.BLL.MediatR.Toponyms.Create;

public record CreateToponymQuery(ToponymDTO Toponym) : IRequest<Result<Unit>>;