using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Validators.Jobs;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Jobs;

public class BaseJobsValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockFailedToValidateLocalizer;
    private readonly MockFieldNamesLocalizer mockFieldNamesLocalizer;
    private readonly BaseJobsValidator validator;

    public BaseJobsValidatorTests()
    {
        this.mockFailedToValidateLocalizer = new MockFailedToValidateLocalizer();
        this.mockFieldNamesLocalizer = new MockFieldNamesLocalizer();
        this.validator = new BaseJobsValidator(this.mockFailedToValidateLocalizer, this.mockFieldNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenJobAreValid()
    {
        // Assert
        var job = this.GetValidJob();

        // Act
        var result = this.validator.Validate(job);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Assert
        var expectedError = this.mockFailedToValidateLocalizer["IsRequired", this.mockFieldNamesLocalizer["Title"]];
        var job = this.GetValidJob();
        job.Title = string.Empty;

        // Act
        var result = this.validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan50()
    {
        // Assert
        var expectedError = this.mockFailedToValidateLocalizer["MaxLength", this.mockFieldNamesLocalizer["Title"], BaseJobsValidator.TitleMaxLength];
        var job = this.GetValidJob();
        job.Title = new string('*', BaseJobsValidator.TitleMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDescriptionLengthIsMoreThan3000()
    {
        // Assert
        var expectedError = this.mockFailedToValidateLocalizer["MaxLength", this.mockFieldNamesLocalizer["Description"], BaseJobsValidator.DescriptionMaxLength];
        var job = this.GetValidJob();
        job.Description = new string('*', BaseJobsValidator.DescriptionMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSalaryIsEmpty()
    {
        // Assert
        var expectedError = this.mockFailedToValidateLocalizer["IsRequired", this.mockFieldNamesLocalizer["Salary"]];
        var job = this.GetValidJob();
        job.Salary = string.Empty;

        // Act
        var result = this.validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Salary)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSalaryLengthMoreThan15()
    {
        // Assert
        var expectedError = this.mockFailedToValidateLocalizer["MaxLength", this.mockFieldNamesLocalizer["Salary"], BaseJobsValidator.SalaryMaxLength];
        var job = this.GetValidJob();
        job.Salary = new string('s', BaseJobsValidator.SalaryMaxLength + 1);

        // Act
        var result = this.validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Salary)
            .WithErrorMessage(expectedError);
    }

    public CreateUpdateJobDto GetValidJob()
    {
        return new JobCreateDto()
        {
            Title = "Fullstack ChatGPT Developer",
            Description = "bla bla bla",
            Salary = "9000$",
            Status = true,
        };
    }
}