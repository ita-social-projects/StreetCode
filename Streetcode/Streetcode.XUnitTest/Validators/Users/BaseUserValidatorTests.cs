using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.Validators.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Users;

public class BaseUserValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly BaseUserValidator validator;

    public BaseUserValidatorTests()
    {
        mockNamesLocalizer = new MockFieldNamesLocalizer();
        mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        validator = new BaseUserValidator(mockValidationLocalizer, mockNamesLocalizer, _mockRepositoryWrapper.Object);
    }

    [Fact]
    public void ShouldReturnSuccessResult_WhenAllFieldsAreValid()
    {
        // Arrange
        var user = GetValidUser();

        // Act
        var result = validator.Validate(user);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ShouldReturnError_WhenExpertisesCountIsMoreThan3()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MustContainAtMostThreeExpertises", mockNamesLocalizer["Expertises"]];
        var user = GetValidUser();
        user.Expertises.Add(new ExpertiseDTO
        {
            Id = 4,
            Title = "testTitle4",
        });

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Expertises)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenAboutYourselfLengthIsMoreThanMaximum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["AboutYourself"], BaseUserValidator.MaxLengthAboutYourself];
        var user = GetValidUser();
        user.AboutYourself = new string('a', BaseUserValidator.MaxLengthAboutYourself + 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AboutYourself)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("Нікнейм1")]
    [InlineData("NAME")]
    [InlineData("!name")]
    public void ShouldReturnError_WhenUserNameIsInvalid(string userName)
    {
        // Arrange
        var expectedError = mockValidationLocalizer["UserNameFormat"];
        var user = GetValidUser();
        user.UserName = userName;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenUserNameIsEmpty()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["UserName"]];
        var user = GetValidUser();
        user.UserName = string.Empty;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenUserNameLengthIsMoreThanMaximum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["UserName"], BaseUserValidator.MaxLengthUserName];
        var user = GetValidUser();
        user.UserName = new string('a', BaseUserValidator.MaxLengthUserName + 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenUserNameLengthIsLessThanMinimum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MinLength", mockNamesLocalizer["UserName"], BaseUserValidator.MinLengthUserName];
        var user = GetValidUser();
        user.UserName = new string('a', BaseUserValidator.MinLengthUserName - 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserName)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("name1")]
    [InlineData("name_2")]
    [InlineData("!name")]
    public void ShouldReturnError_WhenNameIsInvalid(string name)
    {
        // Arrange
        var expectedError = mockValidationLocalizer["NameFormat"];
        var user = GetValidUser();
        user.Name = name;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenNameIsEmpty()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["Name"]];
        var user = GetValidUser();
        user.Name = string.Empty;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenNameLengthIsMoreThanMaximum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["Name"], BaseUserValidator.MaxLengthName];
        var user = GetValidUser();
        user.Name = new string('a', BaseUserValidator.MaxLengthName + 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenNameLengthIsLessThanMinimum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MinLength", mockNamesLocalizer["Name"], BaseUserValidator.MinLengthName];
        var user = GetValidUser();
        user.Name = new string('a', BaseUserValidator.MinLengthName - 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(expectedError);
    }

    [Theory]
    [InlineData("surname1")]
    [InlineData("surname_2")]
    [InlineData("!surname")]
    public void ShouldReturnError_WhenSurnameIsInvalid(string surname)
    {
        // Arrange
        var expectedError = mockValidationLocalizer["SurnameFormat"];
        var user = GetValidUser();
        user.Surname = surname;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSurnameIsEmpty()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["CannotBeEmpty", mockNamesLocalizer["Surname"]];
        var user = GetValidUser();
        user.Surname = string.Empty;

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSurnameLengthIsMoreThanMaximum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MaxLength", mockNamesLocalizer["Surname"], BaseUserValidator.MaxLengthSurname];
        var user = GetValidUser();
        user.Surname = new string('a', BaseUserValidator.MaxLengthSurname + 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public void ShouldReturnError_WhenSurnameLengthIsLessThanMinimum()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MinLength", mockNamesLocalizer["Surname"], BaseUserValidator.MinLengthSurname];
        var user = GetValidUser();
        user.Surname = new string('a', BaseUserValidator.MinLengthSurname - 1);

        // Act
        var result = validator.TestValidate(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Surname)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_WhenImageDoesNotExist()
    {
        // Arrange
        var user = GetValidUser();
        user.AvatarId = 1;
        var expectedError = mockValidationLocalizer["ImageDoesntExist", user.AvatarId];
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);

        // Act
        var result = await validator.TestValidateAsync(user);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AvatarId)
            .WithErrorMessage(expectedError);
    }

    private UpdateUserDTO GetValidUser()
    {
        return new UpdateUserDTO
        {
            Name = "TestName",
            Surname = "TestSurname",
            UserName = "testusername",
            AboutYourself = null,
            AvatarId = null,
            Expertises = new List<ExpertiseDTO>()
            {
                new ()
                {
                    Id = 1,
                    Title = "testTitle1",
                },
                new ()
                {
                    Id = 2,
                    Title = "testTitle2",
                },
                new ()
                {
                    Id = 3,
                    Title = "testTitle3",
                },
            },
            PhoneNumber = null!,
            Email = "testemail",
        };
    }
}