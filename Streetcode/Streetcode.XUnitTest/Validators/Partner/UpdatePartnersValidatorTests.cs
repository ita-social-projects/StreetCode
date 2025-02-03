using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.MediatR.Partners.Update;
using Streetcode.BLL.Validators.Partners;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Partner;

public class UpdatePartnersValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly Mock<PartnerSourceLinkValidator> mockPartnerSourceLinkValidator;
    private readonly Mock<BasePartnersValidator> mockBasePartnerValidator;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly MockNoSharedResourceLocalizer _mockNoSharedResourceLocalizer;
    private readonly MockAlreadyExistLocalizer _mockAlreadyExistLocalizer;

    public UpdatePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(this.mockNamesLocalizer, this.mockValidationLocalizer);
        _mockNoSharedResourceLocalizer = new MockNoSharedResourceLocalizer();
        _mockAlreadyExistLocalizer = new MockAlreadyExistLocalizer();
        this.mockBasePartnerValidator = new Mock<BasePartnersValidator>(this.mockPartnerSourceLinkValidator.Object, this.mockNamesLocalizer, this.mockValidationLocalizer, this._mockNoSharedResourceLocalizer, this._mockRepositoryWrapper.Object);

        this.mockPartnerSourceLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreatePartnerSourceLinkDto>>()))
            .Returns(new ValidationResult());
        this.mockBasePartnerValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDto());
        var updateValidator = new UpdatePartnerValidator(this.mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, mockNamesLocalizer);
        MockHelpers.SetupMockPartnersRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, query.Partner.LogoId);

        // Act
        updateValidator.ValidateAsync(query);

        // Assert
        this.mockBasePartnerValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnError_WhenPartnerWithTheSameLogoIdExist()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDto());
        query.Partner.LogoId = 2;
        query.Partner.Id = 1;
        var expectedError = _mockAlreadyExistLocalizer["PartnerWithFieldAlreadyExist", mockNamesLocalizer["LogoId"], query.Partner.LogoId];
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { LogoId = 2 });

        var validator = new UpdatePartnerValidator(this.mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, mockNamesLocalizer);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Partner)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnSuccess_WhenPartnerWithTheSameLogoIdDoesNotExist()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDto());
        query.Partner.LogoId = 2;
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner());

        var validator = new UpdatePartnerValidator(this.mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, mockNamesLocalizer);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnSuccess_WhenPartnerWithTheSameLogoIdIsTheSamePartner()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDto());
        query.Partner.Id = 2;
        query.Partner.LogoId = 2;
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { Id = 2, LogoId = 2 });

        var validator = new UpdatePartnerValidator(this.mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, mockNamesLocalizer);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }
}