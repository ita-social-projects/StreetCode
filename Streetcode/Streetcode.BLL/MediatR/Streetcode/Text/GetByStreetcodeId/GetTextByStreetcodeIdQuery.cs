using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId
{
    public record GetTextByStreetcodeIdQuery(int streetcodeId) : IRequest<Result<TextDTO>>;
}
