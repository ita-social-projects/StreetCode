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
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockUserSharedResourceLocalizer _mockUserSharedResourceLocalizer;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly Mock<BaseUserValidator> _mockBaseValidator;

    public UpdateUserValidatorTests()
    {
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockUserSharedResourceLocalizer = new MockUserSharedResourceLocalizer();
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();

        _mockBaseValidator = new Mock<BaseUserValidator>(_mockValidationLocalizer, _mockNamesLocalizer, _mockRepositoryWrapper.Object);
        _mockBaseValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<UpdateUserDTO>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public async Task Validate_WhenCalled_ShouldCallBaseValidator()
    {
        // Arrange
        var createValidator = new UpdateUserValidator(_mockBaseValidator.Object, _mockRepositoryWrapper.Object, _mockUserSharedResourceLocalizer, _mockValidationLocalizer, _mockNamesLocalizer);
        var user = GetValidUser();
        var createCommand = new UpdateUserCommand(user);
        MockHelpers.SetupMockUserRepositoryGetFirstOfDefaultAsync(_mockRepositoryWrapper, user.UserName);

        // Act
        await createValidator.TestValidateAsync(createCommand);

        // Assert
        _mockBaseValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<UpdateUserDTO>>(), CancellationToken.None), Times.Once);
    }

    private static UpdateUserDTO GetValidUser()
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
            Id = "FD97D98B-B4B3-45D3-990C-87C41DC28FC0",
        };
    }
}