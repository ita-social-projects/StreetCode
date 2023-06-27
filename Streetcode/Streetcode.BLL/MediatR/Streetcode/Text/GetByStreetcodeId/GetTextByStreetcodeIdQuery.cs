using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;

public record GetTextByStreetcodeIdQuery(int StreetcodeId) : IRequest<Result<TextDTO?>>;