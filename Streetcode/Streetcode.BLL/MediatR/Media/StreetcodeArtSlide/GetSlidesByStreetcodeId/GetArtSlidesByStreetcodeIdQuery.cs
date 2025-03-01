using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public record GetArtSlidesByStreetcodeIdQuery(uint StreetcodeId, ushort FromSlideN, ushort AmoutOfSlides, UserRole? UserRole)
        : IRequest<Result<IEnumerable<StreetcodeArtSlideDTO>>>;
}
