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
            .NotEmpty().MaximumLength(150).WithMessage(localizer["AddressStatisticRecordMaximumLength"]);
    }
}