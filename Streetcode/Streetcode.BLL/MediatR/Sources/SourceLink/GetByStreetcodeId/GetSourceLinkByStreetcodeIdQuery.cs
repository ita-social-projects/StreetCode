using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Sources;

namespace Streetcode.BLL.MediatR.Sources.SourceLink.GetByStreetcodeId
{
    public record GetSourceLinkByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<SourceLinkDTO>>>;
}
