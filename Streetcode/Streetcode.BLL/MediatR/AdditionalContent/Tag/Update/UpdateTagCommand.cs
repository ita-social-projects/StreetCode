using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.Update
{
	public record UpdateTagCommand(UpdateTagDTO tag)
		: IRequest<Result<TagDTO>>;
}
