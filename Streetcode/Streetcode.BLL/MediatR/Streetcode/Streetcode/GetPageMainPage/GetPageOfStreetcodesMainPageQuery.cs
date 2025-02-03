using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetPageMainPage
{
    public record GetPageOfStreetcodesMainPageQuery(ushort page, ushort pageSize)
        : IRequest<Result<IEnumerable<StreetcodeMainPageDto>>>;
}
