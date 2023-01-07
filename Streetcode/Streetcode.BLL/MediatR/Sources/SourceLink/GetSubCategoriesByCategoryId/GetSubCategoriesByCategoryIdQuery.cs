using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;

public record GetSubCategoriesByCategoryIdQuery(int categoryId) : IRequest<Result<IEnumerable<SourceLinkSubCategoryDTO>>>;
