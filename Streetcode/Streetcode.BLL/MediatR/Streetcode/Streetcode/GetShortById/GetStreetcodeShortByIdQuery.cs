using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.GetShortById
{
    public record GetStreetcodeShortByIdQuery(int id) : IRequest<Result<StreetcodeShortDTO>>
    {
    }
}
