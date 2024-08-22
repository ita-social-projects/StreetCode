using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetAllPartnersTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetAllPartnersTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        this.mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this.mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PartnerDTO>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        this.mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPartnerList().Count(), result.Value.Count()));
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExist()
    {
        // Arrange
        var expectedError = "Cannot find any partners";
        this.mockLocalizerCannotFind.Setup(x => x["CannotFindAnyPartners"])
            .Returns(new LocalizedString("CannotFindAnyPartners", expectedError));

        this.mockRepository
            .Setup(x => x.PartnersRepository
                .GetAllAsync(
                    null,
                    It.IsAny<Func<IQueryable<Partner>,
                    IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnersListWithNotExistingId());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);

        this.mockMapper.Verify(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()), Times.Never);
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1,
            },
            new Partner
            {
                Id = 2,
            },
        };

        return partners;
    }

    private static List<Partner> GetPartnersListWithNotExistingId()
    {
        return new List<Partner>();
    }

    private static List<PartnerDTO> GetListPartnerDTO()
    {
        var partnersDTO = new List<PartnerDTO>
        {
            new PartnerDTO
            {
                Id = 1,
            },
            new PartnerDTO
            {
                Id = 2,
            },
        };

        return partnersDTO;
    }
}
