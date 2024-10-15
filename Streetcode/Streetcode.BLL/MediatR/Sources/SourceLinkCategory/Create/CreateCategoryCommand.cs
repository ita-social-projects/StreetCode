using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Create
{
    public record CreateCategoryCommand(SourceLinkCategoryCreateDTO Category)
        : IRequest<Result<CreateSourceLinkCategoryDTO>>;
}
