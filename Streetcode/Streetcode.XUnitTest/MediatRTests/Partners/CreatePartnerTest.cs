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
            this.mockLogger.Object);

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
            this.mockLogger.Object);

        // Act
        var result = await handler.Handle(new CreatePartnerQuery(GetCreatePartnerDto()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
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
            Streetcodes = new List<StreetcodeShortDto>
            {
                new StreetcodeShortDto { Id = 1 },
                new StreetcodeShortDto { Id = 2 },
                new StreetcodeShortDto { Id = 3 },
            },
        };
    }
}
