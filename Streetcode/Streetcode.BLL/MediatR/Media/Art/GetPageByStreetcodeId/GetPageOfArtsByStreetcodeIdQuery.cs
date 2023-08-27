using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Media.Art;

namespace Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId
{
  public record GetPageOfArtsByStreetcodeIdQuery(uint StreetcodeId, ushort Page, ushort PageSize) : IRequest<Result<IEnumerable<ArtDTO>>>;
}
