using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;

public record GetRelatedFigureByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;
