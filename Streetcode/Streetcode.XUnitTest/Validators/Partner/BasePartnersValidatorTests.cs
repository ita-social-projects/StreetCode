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
    private readonly MockFailedToValidateLocalizer _mockValidationLocalizer;
    private readonly MockFieldNamesLocalizer _mockNamesLocalizer;
    private readonly Mock<PartnerSourceLinkValidator> _mockPartnerSourceLinkValidator;
    private readonly BasePartnersValidator _validator;
    private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
    private readonly MockNoSharedResourceLocalizer _mockLocalizerNoShared;

    public BasePartnersValidatorTests()
    {
        _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
        _mockValidationLocalizer = new MockFailedToValidateLocalizer();
        _mockNamesLocalizer = new MockFieldNamesLocalizer();
        _mockLocalizerNoShared = new MockNoSharedResourceLocalizer();
        _mockPartnerSourceLinkValidator = new Mock<PartnerSourceLinkValidator>(_mockNamesLocalizer, _mockValidationLocalizer);
        _mockPartnerSourceLinkValidator.Setup(x => x.Validate(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>()))
            .Returns(new ValidationResult());
        _validator = new BasePartnersValidator(_mockPartnerSourceLinkValidator.Object, _mockNamesLocalizer, _mockValidationLocalizer, _mockLocalizerNoShared, _mockRepositoryWrapper.Object);
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenPartnerIsValid()
    {
        // Arrange
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        // Act
        var result = await _validator.ValidateAsync(partner);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async void ShouldreturnError_WhenTitleIsEmpty()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmpty", _mockNamesLocalizer["Title"]];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Title = string.Empty;

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async Task ShouldReturnFail_WhenImageDoesNotExist()
    {
        // Arrange
        var partner = GetValidPartner();
        var expectedError = _mockValidationLocalizer["ImageDoesntExist", partner.LogoId];
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsyncReturnsNull(_mockRepositoryWrapper);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.LogoId)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Title"], BasePartnersValidator.TitleMaxLength];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Title = new string('t', BasePartnersValidator.TitleMaxLength + 1);

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenDescriptionLengthIsMoreThan450()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["Description"], BasePartnersValidator.DescriptionMaxLength];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.Description = new string('d', BasePartnersValidator.DescriptionMaxLength + 1);

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenUrlTitleLengthIsMoreThan100()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["UrlTitle"], BasePartnersValidator.UrlTitleMaxLength];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.UrlTitle = new string('u', BasePartnersValidator.UrlTitleMaxLength + 1);

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UrlTitle)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenTargetUrlLengthIsMoreThan200()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["MaxLength", _mockNamesLocalizer["TargetUrl"], BasePartnersValidator.UrlMaxLength];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());
        partner.TargetUrl = new string('u', BasePartnersValidator.UrlMaxLength + 1);

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnError_WhenUrlIsMissing()
    {
        // Arrange
        var expectedError = _mockValidationLocalizer["CannotBeEmptyWithCondition", _mockNamesLocalizer["TargetUrl"], _mockNamesLocalizer["UrlTitle"]];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.TargetUrl = string.Empty;

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldReturnSuccessResult_WhenUrlAndUrlTitleIsMissing()
    {
        // Arrange
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());
        partner.TargetUrl = string.Empty;
        partner.UrlTitle = string.Empty;

        // Act
        var result = await _validator.TestValidateAsync(partner);

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
        var expectedError = _mockValidationLocalizer["ValidUrl", _mockNamesLocalizer["TargetUrl"]];
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        partner.TargetUrl = invalidUrl;

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.TargetUrl)
            .WithErrorMessage(expectedError);
    }

    [Fact]
    public async void ShouldCallPartnerSourceLinkValidator()
    {
        // Arrange
        var partner = GetValidPartner();
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, partner.Streetcodes.Select(x => x.Id).ToList());

        // Act
        await _validator.ValidateAsync(partner);

        // Assert
        _mockPartnerSourceLinkValidator.Verify(x => x.ValidateAsync(It.IsAny<ValidationContext<CreatePartnerSourceLinkDTO>>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async void ShouldReturnError_WhenStreetcodesDoesNotExist()
    {
        // Arrange
        var partner = GetValidPartner();
        var expectedError = _mockLocalizerNoShared["NoExistingStreetcodeWithId", $"{partner.Streetcodes.ElementAt(1).Id}"];
        MockHelpers.SetupMockImageRepositoryGetFirstOrDefaultAsync(_mockRepositoryWrapper, partner.LogoId);
        MockHelpers.SetupMockStreetcodeRepositoryFindAll(_mockRepositoryWrapper, new List<int>() { 1, 2 });

        // Act
        var result = await _validator.TestValidateAsync(partner);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Streetcodes.Select(dto => dto.Id).ToList())
            .WithErrorMessage(expectedError);
    }

    private static PartnerCreateUpdateDto GetValidPartner()
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
            Streetcodes = new List<StreetcodeShortDto>()
            {
                new () { Id = 1 },
                new () { Id = 3 },
            },
        };
    }
}