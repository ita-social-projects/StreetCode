using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl
{
    public record GetStreetcodeByTransliterationUrlQuery(string url) : IRequest<Result<StreetcodeDTO>>;
}
