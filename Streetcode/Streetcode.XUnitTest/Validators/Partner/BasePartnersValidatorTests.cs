using FluentValidation;
using FluentValidation.Results;
using FluentValidation.TestHelper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Validators.Partners;
using Streetcode.BLL.Validators.Partners.SourceLinks;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.Validators.Partner;

public class BasePartnersValidatorTests
{
    private readonly MockFailedToValidateLocalizer mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer mockNamesLocalizer;
    private readonly Mock<PartnerSourceLinkValidator> mockPartnerSourceLinkValidator;
    private readonly BasePartnersValidator validator;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly MockNoSharedResourceLocalizer _mockLocalizerNoShared;

    public BasePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        this.mockValidationLocalizer = new MockFailedToValidateLocalizer();
        this.mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockLocalizerNoShared = new MockNoSharedResourceLocalizer();
        this.mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(this.mockNamesLocalizer, this.mockValidationLocalizer);
        this.mockPartnerSourceLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>()))
            .Returns(new ValidationResult());
        this.validator = new BasePartnersValidator(this.mockPartnerSourceLinkValidator.Object, this.mockNamesLocalizer, this.mockValidationLocalizer, _mockLocalizerNoShared, this._mockRepositoryWrapper.Object);
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenPartnerIsValid()
    {
        // Arrange
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());
        // Act
        var result = await this.validator.ValidateAsync(partner);
        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async void ShouldreturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmpty", this.mockNamesLocalizer["Title"]];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Title = string.Empty;

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_WhenImageDoesNotExist()
    {
        // Arrange
        var partner = this.GetValidPartner();
        var expectedError = this.mockValidationLocalizer["ImageDoesntExist", partner.LogoId];
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Title"], BasePartnersValidator.TitleMaxLength];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Title = new string('t', BasePartnersValidator.TitleMaxLength + 1);

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenDescriptionLengthIsMoreThan450()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["Description"], BasePartnersValidator.DescriptionMaxLength];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Description = new string('d', BasePartnersValidator.DescriptionMaxLength + 1);

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenUrlTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["UrlTitle"], BasePartnersValidator.UrlTitleMaxLength];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.UrlTitle = new string('u', BasePartnersValidator.UrlTitleMaxLength + 1);

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UrlTitle)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenTargetUrlLengthIsMoreThan200()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["MaxLength", this.mockNamesLocalizer["TargetUrl"], BasePartnersValidator.UrlMaxLength];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());
        partner.TargetUrl = new string('u', BasePartnersValidator.UrlMaxLength + 1);

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenUrlIsMissing()
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["CannotBeEmptyWithCondition", this.mockNamesLocalizer["TargetUrl"], this.mockNamesLocalizer["UrlTitle"]];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.TargetUrl = string.Empty;

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenUrlAndUrlTitleIsMissing()
    {
        // Arrange
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());
        partner.TargetUrl = string.Empty;
        partner.UrlTitle = string.Empty;

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveAnyValidationError();
    }

    [Theory]
    [InlineData("test")]
    [InlineData("asdjj://sdfsdf.com")]
    [InlineData("ftp://test.com")]
    [InlineData("http:////test.com")]
    [InlineData("http://test..com")]
    public async void ShouldReturnSuccessResult_WhenUrlIsIncorrect(string invalidUrl)
    {
        // Arrange
        var expectedError = this.mockValidationLocalizer["ValidUrl", this.mockNamesLocalizer["TargetUrl"]];
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.TargetUrl = invalidUrl;

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldCallPartnerSourceLinkValidator()
    {
        // Arrange
        var partner = this.GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        // Act
        await this.validator.ValidateAsync(partner);

        // Assert
        this.mockPartnerSourceLinkValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async void ShouldReturnError_WhenStreetcodesDoesNotExist()
    {
        // Arrange
        var partner = this.GetValidPartner();
        var expectedError = this._mockLocalizerNoShared["NoExistingStreetcodeWithId", $"{partner.Streetcodes.ElementAt(1).Id}"];
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, new List<int>() { 1, 2 });

        // Act
        var result = await this.validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcodes.Select(dto => dto.Id).ToList())
            .WithErrorMessage(expectedError);
    }

    private PartnerCreateUpdateDto GetValidPartner()
    {
        return new CreatePartnerDTO()
        {
            Title = "SoftServe Academy",
            Description = "Soft Serve Academy",
            TargetUrl = "https://www.soft-serve.com/",
            UrlTitle = "SoftServe Academy",
            IsVisibleEverywhere = true,
            IsKeyPartner = true,
            LogoId = 3,
            PartnerSourceLinks = new List<CreatePartnerSourceLinkDTO>()
            {
                new CreatePartnerSourceLinkDTO()
                {
                    Id = 1,
                    LogoType = LogoType.Behance,
                    TargetUrl = "http://test.com",
                },
            },
            Streetcodes = new List<StreetcodeShortDTO>()
            {
                new () { Id = 1 },
                new () { Id = 3 },
            },
        };
    }
}