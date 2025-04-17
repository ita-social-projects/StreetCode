using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;

public record GetCategoryByIdQuery(int Id, UserRole? UserRole)
    : IRequest<Result<SourceLinkCategoryDTO>>;