using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

public record GetRelatedFigureByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;
