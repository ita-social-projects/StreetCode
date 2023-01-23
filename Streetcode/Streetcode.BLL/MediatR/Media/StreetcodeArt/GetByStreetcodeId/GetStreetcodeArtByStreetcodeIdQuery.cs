using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId
{
    public record GetStreetcodeArtByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<IEnumerable<StreetcodeArtDTO>>>;
}
