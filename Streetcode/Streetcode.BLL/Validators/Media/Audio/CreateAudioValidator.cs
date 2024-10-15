using FluentValidation;
using Streetcode.BLL.MediatR.Media.Audio.Create;

namespace Streetcode.BLL.Validators.Media.Audio;

public class CreateAudioValidator : AbstractValidator<CreateAudioCommand>
{
    public CreateAudioValidator(BaseAudioValidator baseAudioValidator)
    {
        RuleFor(c => c.Audio).SetValidator(baseAudioValidator);
    }
}