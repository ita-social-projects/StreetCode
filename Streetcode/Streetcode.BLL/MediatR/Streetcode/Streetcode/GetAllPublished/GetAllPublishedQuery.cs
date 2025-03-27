using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllPublished;

public record GetAllPublishedQuery()
    : IRequest<Result<IEnumerable<StreetcodeShortDTO>>>;
