using FluentValidation;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;

namespace Streetcode.BLL.Validators.Streetcode.Text.Term
{
    public class UpdateTermValidator : AbstractValidator<UpdateTermCommand>
    {
        public const int TitleMaxLength = 50;
        public const int DescriptionMaxLength = 500;

        private readonly IRepositoryWrapper _repositoryWrapper;

        public UpdateTermValidator(
            IRepositoryWrapper repositoryWrapper,
            BaseTermValidator baseTermValidator,
            IStringLocalizer<FailedToValidateSharedResource> localizer,
            IStringLocalizer<FieldNamesSharedResource> fieldLocalizer)
        {
            _repositoryWrapper = repositoryWrapper;

            RuleFor(x => x.Term.Title)
                .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Title"]])
                .MaximumLength(TitleMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Title"], TitleMaxLength]);

            RuleFor(x => x.Term.Description)
                .NotEmpty().WithMessage(localizer["IsRequired", fieldLocalizer["Description"]])
                .MaximumLength(DescriptionMaxLength).WithMessage(localizer["MaxLength", fieldLocalizer["Description"], DescriptionMaxLength]);

            RuleFor(x => x.Term.Id)
                .MustAsync(async (id, cancellation) => await TermExistsAsync(id))
                .WithMessage(x => localizer["CannotFindAnyTermWithCorrespondingId", x.Term.Id].Value);
        }

        public async Task<bool> TermExistsAsync(int termId)
        {
            return await _repositoryWrapper.TermRepository.GetFirstOrDefaultAsync(e => e.Id == termId) != null;
        }
    }
}
