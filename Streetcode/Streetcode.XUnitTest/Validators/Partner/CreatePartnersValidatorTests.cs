using FluentValidation;
using FluentValidation.Results;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.Validators.Partners;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Entities.Partners;
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

    public CreatePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        this.mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(this.mockNamesLocalizer, this.mockValidationLocalizer);
        this.mockBasePartnerValidator = new Mock<BasePartnersValidator>(this.mockPartnerSourceLinkValidator.Object, this.mockNamesLocalizer, this.mockValidationLocalizer, _mockRepositoryWrapper.Object);

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
        var createValidator = new CreatePartnerValidator(this.mockBasePartnerValidator.Object, _mockRepositoryWrapper.Object, mockNamesLocalizer, mockValidationLocalizer);
        MockHelpers.SetupMockPartnersRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, query.newPartner.LogoId);

        // Act
        await createValidator.ValidateAsync(query);

        // Assert
        this.mockBasePartnerValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<PartnerCreateUpdateDto>>(), CancellationToken.None), Times.Once);
    }
}