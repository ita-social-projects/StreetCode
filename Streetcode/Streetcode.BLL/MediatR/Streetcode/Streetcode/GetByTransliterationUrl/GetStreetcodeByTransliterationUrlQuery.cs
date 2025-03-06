using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl
{
    public record GetStreetcodeByTransliterationUrlQuery(string Url, UserRole? UserRole)
        : IRequest<Result<StreetcodeDTO>>;
}
