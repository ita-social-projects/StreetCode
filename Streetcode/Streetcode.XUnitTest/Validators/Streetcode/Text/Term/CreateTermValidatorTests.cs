using Streetcode.BLL.Validators.Streetcode.Text.Term;
using Moq;
using Streetcode.XUnitTest.Mocks;
using FluentValidation;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using FluentValidation.Results;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Streetcode.Text.Term
{
    public class CreateTermValidatorTests
    {
        private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
        private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
        private readonly Mock<BaseTermValidator> _mockBaseValidator;

        public CreateTermValidatorTests()
        {
            _mockNamesLocalizer = new MockFieldNamesLocalizer();
            _mockValidationLocalizer = new MockFailedToValidateLocalizer();
            _mockBaseValidator = new Mock<BaseTermValidator>(_mockValidationLocalizer, _mockNamesLocalizer);

            _mockBaseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<TermCreateDTO>>()))
                .Returns(new ValidationResult());
        }

        [Fact]
        public void ShouldCallBaseValidator()
        {
            var query = new CreateTermCommand(new TermCreateDTO
            {
                Title = "Test Title",
                Description = "Test Description",
            });

            var createValidator = new CreateTermValidator(_mockBaseValidator.Object, _mockValidationLocalizer, _mockNamesLocalizer);

            createValidator.Validate(query);

            _mockBaseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<TermCreateDTO>>()), Times.Once);
        }
    }
}
