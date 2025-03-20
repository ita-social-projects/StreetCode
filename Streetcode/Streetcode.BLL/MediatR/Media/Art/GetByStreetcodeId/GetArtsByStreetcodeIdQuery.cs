using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public record GetArtsByStreetcodeIdQuery(int StreetcodeId, UserRole? UserRole)
        : IRequest<Result<IEnumerable<ArtDTO>>>;
}
