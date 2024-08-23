using FluentValidation;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Text;

namespace Streetcode.BLL.Validators.Streetcode;

public class CreateStreetcodeValidator : AbstractValidator<CreateStreetcodeCommand>
{
    public CreateStreetcodeValidator(
        BaseStreetcodeValidator baseStreetcodeValidator,
        BaseTextValidator baseTextValidator,
        BaseSubtitleValidator baseSubtitleValidator,
        BaseTagValidator tagValidator)
    {
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);
        RuleForEach(c => c.Streetcode.Tags).SetValidator(tagValidator);
        RuleForEach(c => c.Streetcode.Subtitles).SetValidator(baseSubtitleValidator);
    }
}