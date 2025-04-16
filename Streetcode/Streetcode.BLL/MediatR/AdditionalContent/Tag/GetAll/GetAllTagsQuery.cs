using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;

namespace Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

public record GetAllTagsQuery(ushort? page = null, ushort? pageSize = null, string? title = null)
    : IRequest<Result<GetAllTagsResponseDTO>>;
