using FluentValidation;
using Streetcode.BLL.MediatR.Media.Audio.Update;

namespace Streetcode.BLL.Validators.Media.Audio;

public class UpdateAudioValidator : AbstractValidator<UpdateAudioCommand>
{
    public UpdateAudioValidator(BaseAudioValidator baseAudioValidator)
    {
        RuleFor(c => c.Audio)
            .SetValidator(baseAudioValidator);
    }
}