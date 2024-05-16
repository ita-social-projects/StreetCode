using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Delete
{
  public record DeleteTagCommand(int id)
        : IRequest<Result<int>>;
}
