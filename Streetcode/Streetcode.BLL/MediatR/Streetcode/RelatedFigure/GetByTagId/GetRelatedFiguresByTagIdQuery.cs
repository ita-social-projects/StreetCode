using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId
{
  public record GetRelatedFiguresByTagIdQuery(int tagId) : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;
}
