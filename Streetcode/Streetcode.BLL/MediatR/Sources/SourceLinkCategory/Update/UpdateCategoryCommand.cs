using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Update
{
    public record UpdateCategoryCommand(SourceLinkDTO Category) : IRequest<Result<Unit>>;
}
