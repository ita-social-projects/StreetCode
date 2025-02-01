using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Analytics;

public class StatisticRecordValidator : AbstractValidator<CreateStatisticRecordCommand>
{
    public const int AddressMaxLength = 150;
    public const int LatitudePrecision = 18;
    public const int LatitudeScale = 4;
    public const int LongtitudePrecision = 18;
    public const int LongtitudeScale = 4;

    public StatisticRecordValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(st => st.StatisticRecordDTO.Address)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Address"]])
            .MaximumLength(AddressMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Address"], AddressMaxLength]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Latitude)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Latitude"]])
            .PrecisionScale(LatitudePrecision, LatitudeScale, true).WithMessage(localizer["InvalidPrecision", fieldLocalizer["Latitude"]]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Longtitude)
            .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Longtitude"]])
            .PrecisionScale(LongtitudePrecision, LongtitudeScale, true).WithMessage(localizer["InvalidPrecision", fieldLocalizer["Longtitude"]]);
    }
}