using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;

namespace Streetcode.BLL.Validators.Analytics;

public class StatisticRecordValidator : AbstractValidator<CreateStatisticRecordCommand>
{
    public StatisticRecordValidator(IStringLocalizer<CreateStatisticRecordCommand> localizer)
    {
        RuleFor(st => st.StatisticRecordDTO.Address)
            .NotEmpty().WithMessage(localizer["AddressRequired"])
            .MaximumLength(150).WithMessage(localizer["AddressStatisticRecordMaximumLength"]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Latitude)
            .NotNull().WithMessage(localizer["LatitudeRequired"])
            .PrecisionScale(18, 4, true).WithMessage(localizer["LatitudeInvalidPrecision"]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Longtitude)
            .NotNull().WithMessage(localizer["LongtitudeRequired"])
            .PrecisionScale(18, 4, true).WithMessage(localizer["LongtitudeInvalidPrecision"]);
    }
}