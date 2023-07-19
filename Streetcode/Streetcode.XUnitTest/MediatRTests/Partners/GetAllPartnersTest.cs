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
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetAllPartnersTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        //Arrange
        _mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        _mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PartnerDTO>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        //Arrange
        _mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPartnerList().Count(), result.Value.Count())
        );
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExist()
    {
        //Arrange
        var expectedError = "Cannot find any partners";

        _mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
            null,
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnersListWithNotExistingId());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);

        _mockMapper.Verify(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()), Times.Never);
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1
            },

            new Partner
            {
                Id = 2
            }
        };

        return partners;
    }

    private static List<Partner>? GetPartnersListWithNotExistingId()
    {
        return null;
    }

    private static List<PartnerDTO> GetListPartnerDTO()
    {
        var partnersDTO = new List<PartnerDTO>
        {
            new PartnerDTO
            {
                Id = 1
            },

            new PartnerDTO
            {
                Id = 2,
            }
        };

        return partnersDTO;
    }
}
