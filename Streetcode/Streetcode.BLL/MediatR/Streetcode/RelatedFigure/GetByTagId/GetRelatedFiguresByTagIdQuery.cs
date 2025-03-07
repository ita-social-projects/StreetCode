using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId;

public record GetRelatedFiguresByTagIdQuery(int TagId, UserRole? UserRole)
    : IRequest<Result<IEnumerable<RelatedFigureDTO>>>;

