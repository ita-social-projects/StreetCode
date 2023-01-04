using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public record GetAllTagsQuery : IRequest<Result<IEnumerable<TagDTO>>>;