using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;

public record GetTagByIdQuery(int Id) : IRequest<Result<TagDTO>>;
