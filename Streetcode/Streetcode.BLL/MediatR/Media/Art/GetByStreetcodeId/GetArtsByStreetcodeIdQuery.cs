using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public record GetArtsByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<IEnumerable<ArtDTO>>>;
}
