using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.BLL.Validators.Streetcode.Text.Term;
using TermEntity = Streetcode.DAL.Entities.Streetcode.TextContent.Term;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using System.Linq.Expressions;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Xunit;
using FluentValidation.TestHelper;

namespace Streetcode.XUnitTest.Validators.Streetcode.Text.Term
{
    public class UpdateTermValidatorTests
    {
        private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
        private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
        private readonly Mock<IRepositoryWrapper> _repositoryWrapper;
        private readonly UpdateTermValidator _validator;

        public UpdateTermValidatorTests()
        {
            _mockNamesLocalizer = new MockFieldNamesLocalizer();
            _mockValidationLocalizer = new MockFailedToValidateLocalizer();
            _repositoryWrapper = new Mock<IRepositoryWrapper>();
            _validator = new UpdateTermValidator(_repositoryWrapper.Object, _mockValidationLocalizer, _mockNamesLocalizer);
        }

        [Fact]
        public async Task SouldReturnSuccessResult_WhenTermExistsAndCommandIsValid()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();

            var result = await _validator.TestValidateAsync(command);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndTitleIsEmpty()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Title = string.Empty;
            var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedError);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndDescriptionIsEmpty()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Description = string.Empty;
            var expectedError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedError);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndTitleExceedsMaxLength()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Title = new string('q', UpdateTermValidator.TitleMaxLength + 1);
            var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], UpdateTermValidator.TitleMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedError);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndDescriptionExceedsMaxLength()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Description = new string('q', UpdateTermValidator.DescriptionMaxLength + 1);
            var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], UpdateTermValidator.DescriptionMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedError);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndTitleAndDescriptionAreEmpty()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Title = string.Empty;
            command.Term.Description = string.Empty;
            var expectedTitleError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];
            var expectedDescriptionError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public async Task ShouldReturnValidationError_WhenTermExistsAndTitleAndDescriptionExceedsMaxLength()
        {
            SetupRepositoryWrapper(1, "Valid title", "Valid description");
            var command = GetValidUpdateTermCommand();
            command.Term.Title = new string('q', UpdateTermValidator.TitleMaxLength + 1);
            command.Term.Description = new string('q', UpdateTermValidator.DescriptionMaxLength + 1);
            var expectedTitleError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], UpdateTermValidator.TitleMaxLength];
            var expectedDescriptionError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], UpdateTermValidator.DescriptionMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndCommandIsValid()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            var expectedError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndTitleIsEmpty()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Title = string.Empty;
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedTitleError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndDescriptionIsEmpty()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Description = string.Empty;
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedDescriptionError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndTitleExceedsMaxLength()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Title = new string('q', UpdateTermValidator.TitleMaxLength + 1);
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedTitleError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], UpdateTermValidator.TitleMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndDescriptionExceedsMaxLength()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Description = new string('q', UpdateTermValidator.DescriptionMaxLength + 1);
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedDescriptionError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], UpdateTermValidator.DescriptionMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndTitleAndDescriptionAreEmpty()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Title = string.Empty;
            command.Term.Description = string.Empty;
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedTitleError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Title"]];
            var expectedDescriptionError = _mockValidationLocalizer["IsRequired", _mockNamesLocalizer["Description"]];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        [Fact]
        public async Task SouldReturnValidationError_WhenTermDoesNotExistAndTitleAndDescriptionExceedMaxLength()
        {
            SetupRepositoryWrapperReturnsNull();
            var command = GetValidUpdateTermCommand();
            command.Term.Title = new string('q', UpdateTermValidator.TitleMaxLength + 1);
            command.Term.Description = new string('q', UpdateTermValidator.DescriptionMaxLength + 1);
            var expectedIdError = _mockValidationLocalizer["CannotFindAnyTermWithCorrespondingId", command.Term.Id].Value;
            var expectedTitleError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], UpdateTermValidator.TitleMaxLength];
            var expectedDescriptionError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], UpdateTermValidator.DescriptionMaxLength];

            var result = await _validator.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(x => x.Term.Id).WithErrorMessage(expectedIdError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Title).WithErrorMessage(expectedTitleError);
            result.ShouldHaveValidationErrorFor(x => x.Term.Description).WithErrorMessage(expectedDescriptionError);
        }

        private void SetupRepositoryWrapperReturnsNull()
        {
            _repositoryWrapper.Setup(x => x.TermRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<TermEntity, bool>>>(),
                    It.IsAny<Func<IQueryable<TermEntity>, IIncludableQueryable<TermEntity, object>>>()))
                .ReturnsAsync(null as TermEntity);
        }

        private void SetupRepositoryWrapper(int id, string title, string description)
        {
            _repositoryWrapper.Setup(x => x.TermRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<TermEntity, bool>>>(),
                    It.IsAny<Func<IQueryable<TermEntity>, IIncludableQueryable<TermEntity, object>>>()))
                .ReturnsAsync(new TermEntity()
                {
                    Id = id,
                    Title = title,
                    Description = description
                });
        }

        public UpdateTermCommand GetValidUpdateTermCommand()
        {
            return new UpdateTermCommand(new TermDto
            {
                Id = 1,
                Title = "Valid title",
                Description = "Valid description",
            });
        }
    }
}
