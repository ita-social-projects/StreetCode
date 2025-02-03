using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public record UpdateCategoryCommand(UpdateSourceLinkCategoryDto Category)
        : IRequest<Result<UpdateSourceLinkCategoryDto>>;
}
