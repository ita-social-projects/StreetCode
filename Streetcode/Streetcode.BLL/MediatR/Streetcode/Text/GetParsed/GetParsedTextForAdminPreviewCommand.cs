using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetParsed
{
    public record GetParsedTextForAdminPreviewCommand(string TextToParse) : IRequest<Result<string>>
    {
    }
}
