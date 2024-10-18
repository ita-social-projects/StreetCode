using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.MediatR.Jobs.Create;
using Streetcode.BLL.MediatR.Jobs.Update;
using Streetcode.BLL.Validators.Jobs;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Jobs;

public class UpdateJobsValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;

    public UpdateJobsValidatorTests()
    {
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
    }

    [Fact]
    public void UpdateJobsValidator_ShouldCallBaseValidator()
    {
        // Arrange
        var baseValidator =
            new Mock<BaseJobsValidator>(this.mockValidationLocalizer, this.mockNamesLocalizer);
        baseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateJobDto>>()))
            .Returns(new ValidationResult());
        var updateValidator = new UpdateJobsValidator(baseValidator.Object);
        var updateCommand = new UpdateJobCommand(new JobUpdateDto()
        {
            Id = 1,
            Title = "Fullstack ChatGPT Developer",
            Description = "bla bla bla",
            Salary = "9000$",
            Status = true,
        });

        // Act
        updateValidator.TestValidate(updateCommand);

        // Assert
        baseValidator.Verify(x => x.Validate(It.IsAny<ValidationContext<CreateUpdateJobDto>>()), Times.Once);
    }
}