using FluentResults;
using MediatR;

namespace Streetcode.BLL.MediatR.Streetcode.Text.UpdateParsed;

public record UpdateParsedTextForAdminPreviewCommand(string TextToParse)
    : IRequest<Result<string>>;
