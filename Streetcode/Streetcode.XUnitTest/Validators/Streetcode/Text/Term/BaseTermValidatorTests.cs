using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Validators.Streetcode.Text.Term;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Text.Term
{
    public class BaseTermValidatorTests
    {
        private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
        private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
        private readonly BaseTermValidator _validator;

        public BaseTermValidatorTests()
        {
            _mockNamesLocalizer = new MockFieldNamesLocalizer();
            _mockValidationLocalizer = new MockFailedToValidateLocalizer();
            _validator = new BaseTermValidator(_mockValidationLocalizer, _mockNamesLocalizer);
        }

        [Fact]
        public void SouldReturnSuccessResult_WhenTermCreateDtoIsValid()
        {
            var validTerm = GetValidTermCreateDto();

            var result = _validator.TestValidate(validTerm);

            Assert.True(result.IsValid);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenTitleIsEmpty()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Title = string.Empty;
            var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage(expectedError);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenDescriptionIsEmpty()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Description = string.Empty;
            var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage(expectedError);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenTitleExceedsMaxLength()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Title = new string('q', BaseTermValidator.TitleMaxLength + 1);
            var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseTermValidator.TitleMaxLength];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage(expectedError);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenDescriptionExceedsMaxLength()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Description = new string('q', BaseTermValidator.DescriptionMaxLength + 1);
            var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], BaseTermValidator.DescriptionMaxLength];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage(expectedError);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenTitleAndDescriptionAreEmpty()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Title = string.Empty;
            invalidTerm.Description = string.Empty;

            var expectedTitleError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];
            var expectedDescriptionError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public void ShouldReturnValidationError_WhenTitleAndDescriptionExceedMaxLength()
        {
            var invalidTerm = GetValidTermCreateDto();
            invalidTerm.Title = new string('q', BaseTermValidator.TitleMaxLength + 1);
            invalidTerm.Description = new string('q', BaseTermValidator.DescriptionMaxLength + 1);

            var expectedTitleError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BaseTermValidator.TitleMaxLength];
            var expectedDescriptionError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], BaseTermValidator.DescriptionMaxLength];

            var result = _validator.TestValidate(invalidTerm);

            result.ShouldHaveValidationErrorFor(x => x.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Description).WithErrorMessage(expectedDescriptionError);
        }

        public static TermCreateDTO GetValidTermCreateDto()
        {
            return new TermCreateDTO()
            {
                Title = "Title Test",
                Description = "Description Test",
            };
        }
    }
}
