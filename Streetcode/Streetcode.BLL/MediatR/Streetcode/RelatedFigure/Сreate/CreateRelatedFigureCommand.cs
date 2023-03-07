using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;

public record CreateRelatedFigureCommand(int Id1, int Id2) : IRequest<Result<Unit>>;