using FluentResults;
using MediatR;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetAll;

public record GetAllTextsQuery : IRequest<Result<IEnumerable<TextDTO>>>;