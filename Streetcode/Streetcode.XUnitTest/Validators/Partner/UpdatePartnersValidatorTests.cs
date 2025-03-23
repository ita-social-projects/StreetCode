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
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly Mock<PartnerSourceLinkValidator> _mockPartnerSourceLinkValidator;
    private readonly Mock<BasePartnersValidator> _mockBasePartnerValidator;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly MockNoSharedResourceLocalizer _mockNoSharedResourceLocalizer;
    private readonly MockAlreadyExistLocalizer _mockAlreadyExistLocalizer;

    public UpdatePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(_mockNamesLocalizer, _mockValidationLocalizer);
        _mockNoSharedResourceLocalizer = new MockNoSharedResourceLocalizer();
        _mockAlreadyExistLocalizer = new MockAlreadyExistLocalizer();
        _mockBasePartnerValidator = new Mock<BasePartnersValidator>(_mockPartnerSourceLinkValidator.Object, _mockNamesLocalizer, _mockValidationLocalizer, _mockNoSharedResourceLocalizer, _mockRepositoryWrapper.Object);

        _mockPartnerSourceLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>()))
            .Returns(new ValidationResult());
        _mockBasePartnerValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>()))
            .Returns(new ValidationResult());
    }

    [Fact]
    public void ShouldCallBaseValidator()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDTO());
        var updateValidator = new UpdatePartnerValidator(_mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, _mockNamesLocalizer);
        MockHelpers.SetupMockPartnersRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, query.Partner.LogoId);

        // Act
        updateValidator.ValidateAsync(query);

        // Assert
        _mockBasePartnerValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnError_WhenPartnerWithTheSameLogoIdExist()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDTO());
        query.Partner.LogoId = 2;
        query.Partner.Id = 1;
        var expectedError = _mockAlreadyExistLocalizer["PartnerWithFieldAlreadyExist", _mockNamesLocalizer["LogoId"], query.Partner.LogoId];
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { LogoId = 2 });

        var validator = new UpdatePartnerValidator(_mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, _mockNamesLocalizer);

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
        var query = new UpdatePartnerQuery(new UpdatePartnerDTO());
        query.Partner.LogoId = 2;
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner());

        var validator = new UpdatePartnerValidator(_mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, _mockNamesLocalizer);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task BeUniqueImageId_ShouldReturnSuccess_WhenPartnerWithTheSameLogoIdIsTheSamePartner()
    {
        // Arrange
        var query = new UpdatePartnerQuery(new UpdatePartnerDTO());
        query.Partner.Id = 2;
        query.Partner.LogoId = 2;
        _mockRepositoryWrapper.Setup(x => x.PartnersRepository.GetSingleOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.Partners.Partner, bool>>>(), null))
            .ReturnsAsync(new DAL.Entities.Partners.Partner() { Id = 2, LogoId = 2 });

        var validator = new UpdatePartnerValidator(_mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, _mockAlreadyExistLocalizer, _mockNamesLocalizer);

        // Assert
        var result = await validator.TestValidateAsync(query);

        // Assert
        Assert.True(result.IsValid);
    }
}