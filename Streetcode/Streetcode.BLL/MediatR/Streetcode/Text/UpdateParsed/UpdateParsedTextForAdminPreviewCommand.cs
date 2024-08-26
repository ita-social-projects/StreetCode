using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetParsed
{
    public record UpdateParsedTextForAdminPreviewCommand(string TextToParse): IRequest<Result<string>>
    {
    }
}
