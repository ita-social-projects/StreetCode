using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId
{
  public record GetStreetcodeArtByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
        : IRequest<Result<IEnumerable<StreetcodeArtDTO>>>;
}
