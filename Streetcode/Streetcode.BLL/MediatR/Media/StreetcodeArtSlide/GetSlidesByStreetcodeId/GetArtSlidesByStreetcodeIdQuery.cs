using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public record GetArtSlidesByStreetcodeIdQuery(uint StreetcodeId, ushort FromSlideN, ushort AmoutOfSlides)
        : IRequest<Result<IEnumerable<StreetcodeArtSlideDTO>>>;
}
