using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetLastWithOffset
{
  public record GetLastWithOffsetQuery(int offset) : IRequest<Result<StreetcodeMainPageDTO>>;
}
