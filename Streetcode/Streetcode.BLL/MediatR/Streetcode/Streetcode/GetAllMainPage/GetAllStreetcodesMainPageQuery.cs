using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllStreetcodesMainPage
{
    public record GetAllStreetcodesMainPageQuery : IRequest<Result<IEnumerable<StreetcodeMainPageDTO>>>;
}
