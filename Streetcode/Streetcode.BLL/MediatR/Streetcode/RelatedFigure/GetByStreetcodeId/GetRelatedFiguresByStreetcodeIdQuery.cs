using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public record GetRelatedFigureByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;
