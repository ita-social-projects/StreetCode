using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.Validators.Partners;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Partner;

public class CreatePartnersValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly Mock<PartnerSourceLinkValidator> mockPartnerSourceLinkValidator;
    private readonly Mock<BasePartnersValidator> mockBasePartnerValidator;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly MockAlreadyExistLocalizer _mockAlreadyExistLocalizer;
    private readonly MockFieldNamesLocalizer _mockLocalizerFieldNames;
    private readonly MockNoSharedResourceLocalizer _mockLocalizerNoShared;
    private readonly CreatePartnerValidator _validator;

    public CreatePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockAlreadyExistLocalizer = new MockAlreadyExistLocalizer();
        _mockLocalizerFieldNames = new MockFieldNamesLocalizer();
        _mockLocalizerNoShared = new MockNoSharedResourceLocalizer();
        
        this.mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(this.mockNamesLocalizer, this.mockValidationLocalizer);
        this.mockBasePartnerValidator = new Mock<BasePartnersValidator>(this.mockPartnerSourceLinkValidator.Object, this.mockNamesLocalizer, this.mockValidationLocalizer, _mockLocalizerNoShared, this._mockRepositoryWrapper.Object);
        
        this.mockPartnerSourceLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>()))
            .Returns(new ValidationResult());
        this.mockBasePartnerValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public async void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new CreatePartnerQuery(new CreatePartnerDTO());
        var createValidator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, mockValidationLocalizer, mockNamesLocalizer, _mockAlreadyExistLocalizer, _mockLocalizerFieldNames, _mockRepositoryWrapper.Object);
        MockHelpers.SetupMockPartnersRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, query.newPartner.LogoId);

        // Act
        await createValidator.ValidateAsync(query);

        // Assert
        this.mockBasePartnerValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task BeUniqueTitle_ShouldReturnError_WhenTitleExist()
    {
        // Arrange
        var expectedError = mockValidationLocalizer["MustBeUnique", mockNamesLocalizer["Title"]];
        var query = new CreatePartnerQuery(new CreatePartnerDTO());
        query.newPartner.Title = "NonUniqueTitle";
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { Title = "NonUniqueTitle" });

        var validator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, mockValidationLocalizer, mockNamesLocalizer, _mockAlreadyExistLocalizer, _mockLocalizerFieldNames, _mockRepositoryWrapper.Object);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.newPartner.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task BeUniqueTitle_ShouldReturnSuccessResult_WhenTitleDoesNotExist()
    {
        // Arrange
        var query = new CreatePartnerQuery(new CreatePartnerDTO());
        query.newPartner.Title = "UniqueTitle";
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync((DAL.Entities.Partners.Partner)null);

        var validator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, mockValidationLocalizer, mockNamesLocalizer, _mockAlreadyExistLocalizer, _mockLocalizerFieldNames, _mockRepositoryWrapper.Object);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnError_WhenPartnerWithTheSameLogoIdExist()
    {
        // Arrange
        var query = new CreatePartnerQuery(new CreatePartnerDTO());
        query.newPartner.LogoId = 2;
        var expectedError = _mockAlreadyExistLocalizer["PartnerWithFieldAlreadyExist", _mockLocalizerFieldNames["LogoId"], query.newPartner.LogoId];
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { LogoId = 2 });

        var validator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, mockValidationLocalizer, mockNamesLocalizer, _mockAlreadyExistLocalizer, _mockLocalizerFieldNames, _mockRepositoryWrapper.Object);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.newPartner.LogoId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnSuccessResult_WhenPartnerWithTheSameLogoIdDoesNotExist()
    {
        // Arrange
        var query = new CreatePartnerQuery(new CreatePartnerDTO());
        query.newPartner.LogoId = 2;
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync((DAL.Entities.Partners.Partner)null);

        var validator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, mockValidationLocalizer, mockNamesLocalizer, _mockAlreadyExistLocalizer, _mockLocalizerFieldNames, _mockRepositoryWrapper.Object);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }
}