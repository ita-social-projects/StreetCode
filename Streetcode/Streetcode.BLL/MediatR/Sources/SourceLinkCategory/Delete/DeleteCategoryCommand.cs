using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.Delete
{
    public record DeleteCategoryCommand(int Id)
        : IRequest<Result<Unit>>;
}
