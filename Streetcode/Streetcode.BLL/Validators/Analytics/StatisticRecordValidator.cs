using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.MediatR.Analytics.StatisticRecord.Create;
using Streetcode.BLL.SharedResource;

namespace Streetcode.BLL.Validators.Analytics;

public class StatisticRecordValidator : AbstractValidator<CreateStatisticRecordCommand>
{
    private const int AddressMaxLength = 150;
    public StatisticRecordValidator(IStringLocalizer<FailedToValidateSharedResource> localizer, IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
    {
        RuleFor(st => st.StatisticRecordDTO.Address)
            .NotEmpty().WithMessage(localizer["CannotBeEmpty", fieldLocalizer["Address"]])
            .MaximumLength(AddressMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Address"], AddressMaxLength]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Latitude)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Latitude"]])
            .PrecisionScale(18, 4, true).WithMessage(localizer["InvalidPrecision", fieldLocalizer["Latitude"]]);

        RuleFor(st => st.StatisticRecordDTO.StreetcodeCoordinate.Longtitude)
            .NotNull().WithMessage(localizer["IsRequired", fieldLocalizer["Longtitude"]])
            .PrecisionScale(18, 4, true).WithMessage(localizer["InvalidPrecision", fieldLocalizer["Longtitude"]]);
    }
}