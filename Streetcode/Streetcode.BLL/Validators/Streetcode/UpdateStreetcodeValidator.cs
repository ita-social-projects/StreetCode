using FluentValidation;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Streetcode.Text;

namespace Streetcode.BLL.Validators.Streetcode;

public class UpdateStreetcodeValidator : AbstractValidator<UpdateStreetcodeCommand>
{
    public UpdateStreetcodeValidator(
        BaseStreetcodeValidator baseStreetcodeValidator,
        BaseTextValidator baseTextValidator,
        BaseTagValidator tagValidator)
    {
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);
        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);
        RuleForEach(c => c.Streetcode.Tags).SetValidator(tagValidator);
    }
}