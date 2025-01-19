using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class CreatePartnerTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> mockLocalizerNoShared;
    private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockLocalizerFieldNames;
    private readonly Mock<IStringLocalizer<AlreadyExistSharedResource>> mockLocalizerAlreadyExist;
    private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFailedToCreate;

    public CreatePartnerTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
        this.mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        this.mockLocalizerAlreadyExist = new Mock<IStringLocalizer<AlreadyExistSharedResource>>();
        this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this.mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDto());

        this.mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this.mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockLogger.Object,
            this.mockLocalizerNoShared.Object,
            this.mockLocalizerFieldNames.Object,
            this.mockLocalizerAlreadyExist.Object,
            this.mockLocalizerFailedToCreate.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDto()), CancellationToken.None);

        // Assert
        Assert.IsType<PartnerDTO>(result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
    {
        // Arrange
        var testPartner = GetPartner();

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        this.mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDto());

        this.mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        this.mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        this.mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockLogger.Object,
            this.mockLocalizerNoShared.Object,
            this.mockLocalizerFieldNames.Object,
            this.mockLocalizerAlreadyExist.Object,
            this.mockLocalizerFailedToCreate.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDto()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldFail_WhenPartnerWithLogoIdAlreadyExists()
    {
        // Arrange
        var testPartner = GetPartner();
        var duplicateLogoId = testPartner.LogoId;
        var createPartnerDto = GetCreatePartnerDto();
        createPartnerDto.LogoId = duplicateLogoId;

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(new Partner { LogoId = duplicateLogoId });

        this.mockRepository.Setup(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync(testPartner);

        this.mockLocalizerFieldNames.Setup(x => x["LogoId"])
            .Returns(new LocalizedString("LogoId", "Logo Id"));

        this.mockLocalizerAlreadyExist.Setup(x => x["PartnerWithFieldAlreadyExist", It.IsAny<object[]>()])
            .Returns((string key, object[] args) =>
                new LocalizedString(key, $"Partner with field '{args[0]}' and its value '{args[1]}' already exists"));

        var handler = new CreatePartnerHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockLogger.Object,
            this.mockLocalizerNoShared.Object,
            this.mockLocalizerFieldNames.Object,
            this.mockLocalizerAlreadyExist.Object,
            this.mockLocalizerFailedToCreate.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(createPartnerDto), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
        Assert.Equal($"Partner with field 'Logo Id' and its value '{GetPartnerDto().LogoId}' already exists", result.Errors[0].Message);

        this.mockRepository.Verify(x => x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null), Times.Once);
        this.mockRepository.Verify(x => x.PartnersRepository.CreateAsync(It.IsAny<Partner>()), Times.Never);
        this.mockRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }


    [Fact]
    public async Task ShouldFail_WhenStreetcodesContainNonExistentIds()
    {
        var createPartnerDto = GetCreatePartnerDtoWithStreetcodes();

        createPartnerDto.Streetcodes = new List<StreetcodeShortDTO>
        {
            new () { Id = 1 },
            new () { Id = 2 },
            new () { Id = 3 },
        };

        var existingStreetcodes = new List<StreetcodeContent>
        {
            new () { Id = 1, Title = "Streetcode 1" },
            new () { Id = 2, Title = "Streetcode 2" },
        };

        this.mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(new Partner());

        this.mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(existingStreetcodes);

        this.mockRepository.Setup(x =>
                x.PartnersRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Partner, bool>>>(), null))
            .ReturnsAsync((Partner)null);

        this.mockLocalizerNoShared.Setup(x => x["NoExistingStreetcodeWithId", It.IsAny<object[]>()])
            .Returns((string key, object[] args) =>
                new LocalizedString(key, $"No existing streetcode with ID(s): {args[0]}"));

        var handler = new CreatePartnerHandler(
            this.mockRepository.Object,
            this.mockMapper.Object,
            this.mockLogger.Object,
            this.mockLocalizerNoShared.Object,
            this.mockLocalizerFieldNames.Object,
            this.mockLocalizerAlreadyExist.Object,
            this.mockLocalizerFailedToCreate.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(createPartnerDto), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Single(result.Errors);
        Assert.Equal("No existing streetcode with ID(s): 3", result.Errors[0].Message);

        this.mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Once);
        this.mockRepository.Verify(x => x.PartnersRepository.CreateAsync(It.IsAny<Partner>()), Times.Never);
        this.mockRepository.Verify(x => x.SaveChangesAsync(), Times.Never);
    }


    private static Partner GetPartner()
    {
        return new Partner
        {
            Id = 1,
            Title = "Existing Partner",
            LogoId = 100,
            IsKeyPartner = true,
            IsVisibleEverywhere = false,
        };
    }

    private static CreatePartnerDTO GetCreatePartnerDto()
    {
        return new CreatePartnerDTO
        {
            Title = "New Partner",
            LogoId = 100,
            IsKeyPartner = false,
            IsVisibleEverywhere = true,
        };
    }

    private static PartnerDTO GetPartnerDto()
    {
        return new PartnerDTO
        {
            Title = "New Partner",
            LogoId = 100,
            IsKeyPartner = false,
            IsVisibleEverywhere = true,
        };
    }

    private static CreatePartnerDTO GetCreatePartnerDtoWithStreetcodes()
    {
        return new CreatePartnerDTO
        {
            Title = "New Partner",
            LogoId = 100,
            IsKeyPartner = false,
            IsVisibleEverywhere = true,
            Streetcodes = new List<StreetcodeShortDTO>
            {
                new StreetcodeShortDTO { Id = 1 },
                new StreetcodeShortDTO { Id = 2 },
                new StreetcodeShortDTO { Id = 3 },
            },
        };
    }
}
