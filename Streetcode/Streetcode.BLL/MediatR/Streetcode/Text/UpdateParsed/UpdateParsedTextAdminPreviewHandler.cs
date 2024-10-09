using FluentResults;
using MediatR;
using Streetcode.BLL.Interfaces.Text;

namespace Streetcode.BLL.MediatR.Streetcode.Text.GetParsed
{
    public class UpdateParsedTextAdminPreviewHandler : IRequestHandler<UpdateParsedTextForAdminPreviewCommand, Result<string>>
    {
        private readonly ITextService _textService;

        public UpdateParsedTextAdminPreviewHandler(ITextService textService)
        {
            _textService = textService;
        }

        public async Task<Result<string>> Handle(UpdateParsedTextForAdminPreviewCommand request, CancellationToken cancellationToken)
        {
            string parsedText = await _textService.AddTermsTag(request.TextToParse);
            return parsedText == null ? Result.Fail(new Error("text was not parsed successfully")) : Result.Ok(parsedText);
        }
    }
}
