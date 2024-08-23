using FluentValidation;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.Validators.AdditionalContent.Tag;
using Streetcode.BLL.Validators.Common;
using Streetcode.BLL.Validators.Streetcode.Facts;
using Streetcode.BLL.Validators.Streetcode.Subtitles;
using Streetcode.BLL.Validators.Streetcode.Text;
using Streetcode.BLL.Validators.Streetcode.Video;

namespace Streetcode.BLL.Validators.Streetcode;

public class UpdateStreetcodeValidator : AbstractValidator<UpdateStreetcodeCommand>
{
    public UpdateStreetcodeValidator(
        BaseStreetcodeValidator baseStreetcodeValidator,
        BaseTextValidator baseTextValidator,
        BaseSubtitleValidator baseSubtitleValidator,
        BaseTagValidator tagValidator,
        BaseFactValidator baseFactValidator,
        BaseVideoValidator videoValidator)
    {
        RuleFor(c => c.Streetcode).SetValidator(baseStreetcodeValidator);

        RuleFor(c => c.Streetcode.TransactionLink!.Url)
            .MustBeValidUrl()
            .When(c => c.Streetcode.TransactionLink != null);

        RuleFor(c => c.Streetcode)
            .Must(HasVideoWithTitle).When(c => c.Streetcode.Videos != null)
            .WithMessage("The 'Title' key for the video is empty or missing");
        RuleForEach(c => c.Streetcode.Videos)
            .SetValidator(videoValidator);

        RuleFor(c => c.Streetcode.Text).SetValidator(baseTextValidator);
        RuleForEach(c => c.Streetcode.Tags).SetValidator(tagValidator);
        RuleForEach(c => c.Streetcode.Subtitles).SetValidator(baseSubtitleValidator);
        RuleForEach(c => c.Streetcode.Facts).SetValidator(baseFactValidator);
    }

    private bool HasVideoWithTitle(StreetcodeUpdateDTO streetcode)
    {
        bool hasTitle = !string.IsNullOrWhiteSpace(streetcode.Text?.Title);
        bool hasVideo = streetcode.Videos != null;
        return hasTitle && hasVideo;
    }
}