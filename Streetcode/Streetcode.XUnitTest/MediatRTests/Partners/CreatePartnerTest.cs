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
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<NoSharedResource>> _mockLocalizerNoShared;
    private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> _mockLocalizerFieldNames;
    private readonly Mock<IStringLocalizer<AlreadyExistSharedResource>> _mockLocalizerAlreadyExist;
    private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;

    public CreatePartnerTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerNoShared = new Mock<IStringLocalizer<NoSharedResource>>();
        _mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        _mockLocalizerAlreadyExist = new Mock<IStringLocalizer<AlreadyExistSharedResource>>();
        _mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testPartner = GetPartner();

        _mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        _mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDto());

        _mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        _mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        _mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

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

        _mockMapper.Setup(x => x.Map<Partner>(It.IsAny<CreatePartnerDTO>()))
            .Returns(testPartner);
        _mockMapper.Setup(x => x.Map<PartnerDTO>(It.IsAny<Partner>()))
            .Returns(GetPartnerDto());

        _mockRepository.Setup(x => x.PartnersRepository.CreateAsync(It.Is<Partner>(y => y.Id == testPartner.Id)))
            .ReturnsAsync(testPartner);
        _mockRepository.Setup(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null))
            .ReturnsAsync(new List<StreetcodeContent>());
        _mockRepository.Setup(x => x.SaveChanges())
            .Returns(1);

        var handler = new CreatePartnerHandler(
            _mockRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object);

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
            Streetcodes = new List<StreetcodeShortDTO>
            {
                new StreetcodeShortDTO { Id = 1 },
                new StreetcodeShortDTO { Id = 2 },
                new StreetcodeShortDTO { Id = 3 },
            },
        };
    }
}
