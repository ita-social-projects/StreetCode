using FluentValidation.TestHelper;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.BLL.Validators.Jobs;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Jobs;

public class BaseJobsValidatorTests
{
    private readonly MockFailedToValidateLocalizer _mockFailedToValidateLocalizer;
    private readonly MockFieldNamesLocalizer _mockFieldNamesLocalizer;
    private readonly BaseJobsValidator _validator;

    public BaseJobsValidatorTests()
    {
        _mockFailedToValidateLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldNamesLocalizer = new MockFieldNamesLocalizer();
        _validator = new BaseJobsValidator(_mockFailedToValidateLocalizer, _mockFieldNamesLocalizer);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenJobAreValid()
    {
        // Assert
        var job = GetValidJob();

        // Act
        var result = _validator.Validate(job);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleIsEmpty()
    {
        // Assert
        var expectedError = _mockFailedToValidateLocalizer["IsRequired", _mockFieldNamesLocalizer["Title"]];
        var job = GetValidJob();
        job.Title = string.Empty;

        // Act
        var result = _validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenTitleLengthIsMoreThan50()
    {
        // Assert
        var expectedError = _mockFailedToValidateLocalizer["MaxLength", _mockFieldNamesLocalizer["Title"], BaseJobsValidator.TitleMaxLength];
        var job = GetValidJob();
        job.Title = new string('*', BaseJobsValidator.TitleMaxLength + 1);

        // Act
        var result = _validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenDescriptionLengthIsMoreThan3000()
    {
        // Assert
        var expectedError = _mockFailedToValidateLocalizer["MaxLength", _mockFieldNamesLocalizer["Description"], BaseJobsValidator.DescriptionMaxLength];
        var job = GetValidJob();
        job.Description = new string('*', BaseJobsValidator.DescriptionMaxLength + 1);

        // Act
        var result = _validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSalaryIsEmpty()
    {
        // Assert
        var expectedError = _mockFailedToValidateLocalizer["IsRequired", _mockFieldNamesLocalizer["Salary"]];
        var job = GetValidJob();
        job.Salary = string.Empty;

        // Act
        var result = _validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Salary)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSalaryLengthMoreThan15()
    {
        // Assert
        var expectedError = _mockFailedToValidateLocalizer["MaxLength", _mockFieldNamesLocalizer["Salary"], BaseJobsValidator.SalaryMaxLength];
        var job = GetValidJob();
        job.Salary = new string('s', BaseJobsValidator.SalaryMaxLength + 1);

        // Act
        var result = _validator.TestValidate(job);

        // Assert
        result.ShouldHaveValidationErrorFor(j => j.Salary)
            .WithErrorMessage(expectedError);
    }

    private static CreateUpdateJobDto GetValidJob()
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