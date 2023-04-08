using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Create
{
    public record CreateTagQuery(CreateTagDTO tag) : IRequest<Result<TagDTO>>;
}
