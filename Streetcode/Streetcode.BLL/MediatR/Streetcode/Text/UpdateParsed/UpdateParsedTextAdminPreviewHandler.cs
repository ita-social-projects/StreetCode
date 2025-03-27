using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Text;

namespace Streetcode.BLL.MediatR.Streetcode.Text.UpdateParsed;

public class UpdateParsedTextAdminPreviewHandler : IRequestHandler<UpdateParsedTextForAdminPreviewCommand, Result<string>>
{
    private readonly ITextService _textService;

    public UpdateParsedTextAdminPreviewHandler(ITextService textService)
    {
        _textService = textService;
    }

    public async Task<Result<string>> Handle(UpdateParsedTextForAdminPreviewCommand request, CancellationToken cancellationToken)
    {
        var parsedText = await _textService.AddTermsTag(request.TextToParse);

        return Result.Ok(parsedText);
    }
}
