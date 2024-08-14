using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
  public record CreateTagCommand(CreateTagDTO tag) : IRequest<Result<TagDTO>>;
}
