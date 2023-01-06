using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
    public record GetArtByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<ArtDTO>>>;
}
