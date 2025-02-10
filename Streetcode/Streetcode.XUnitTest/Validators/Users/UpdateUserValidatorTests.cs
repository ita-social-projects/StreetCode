using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.DTO.Users.Expertise;
using Streetcode.BLL.MediatR.Users.Update;
using Streetcode.BLL.Validators.Users;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Users;

public class UpdateUserValidatorTests
{
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockUserSharedResourceLocalizer mockUserSharedResourceLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<BaseUserValidator> mockBaseValidator;

    public UpdateUserValidatorTests()
    {
        mockNamesLocalizer = new MockFieldNamesLocalizer();
        mockValidationLocalizer = new MockFailedToValidateLocalizer();
        mockUserSharedResourceLocalizer = new MockUserSharedResourceLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        mockBaseValidator = new Mock<BaseUserValidator>(mockValidationLocalizer, mockNamesLocalizer, _mockRepositoryWrapper.Object);
        mockBaseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<UpdateUserDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public async Task Validate_WhenCalled_ShouldCallBaseValidator()
    {
        // Arrange
        var createValidator = new UpdateUserValidator(mockBaseValidator.Object, _mockRepositoryWrapper.Object, mockUserSharedResourceLocalizer);
        var user = GetValidUser();
        var createCommand = new UpdateUserCommand(user);
        MockHelpers.SetupMockUserRepositoryGetFirstOfDefaultAsync(_mockRepositoryWrapper, user.UserName);

        // Act
        await createValidator.TestValidateAsync(createCommand);

        // Assert
        mockBaseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateUserDTO>>(), CancellationToken.None), Times.Once);
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