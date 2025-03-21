using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetAllPartnersTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
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
        // Arrange
        SetupPaginatedRepository(GetPartnerList());
        SetupMapper(GetListPartnerDto());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PartnerDTO>>(result.Value.Partners)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        SetupPaginatedRepository(GetPartnerList());
        SetupMapper(GetListPartnerDto());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPartnerList().Count(), result.Value.Partners.Count())
        );
    }

    [Fact]
    public async Task Handler_Returns_Correct_PageSize()
    {
        // Arrange
        ushort pageSize = 3;
        SetupPaginatedRepository(GetPartnerList().Take(pageSize));
        SetupMapper(GetListPartnerDto().Take(pageSize).ToList());

        var handler = new GetAllPartnersHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(page: 1, pageSize: pageSize), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<List<PartnerDTO>>(result.Value.Partners),
            () => Assert.Equal(pageSize, result.Value.Partners.Count()));
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
        new Partner
        {
            Id = 3,
        },
        new Partner
        {
            Id = 4,
        },
        new Partner
        {
            Id = 5,
        },
    };

        return partners;
    }

    private static List<PartnerDTO> GetListPartnerDto()
    {
        var partnersDto = new List<PartnerDTO>
    {
        new PartnerDTO
        {
            Id = 1,
        },
        new PartnerDTO
        {
            Id = 2,
        },
        new PartnerDTO
        {
            Id = 3,
        },
        new PartnerDTO
        {
            Id = 4,
        },
        new PartnerDTO
        {
            Id = 5,
        },
    };

        return partnersDto;
    }

    private void SetupPaginatedRepository(IEnumerable<Partner> returnList)
    {
        _mockRepository.Setup(repo => repo.PartnersRepository.GetAllPaginated(
            It.IsAny<ushort?>(),
            It.IsAny<ushort?>(),
            It.IsAny<Expression<Func<Partner, Partner>>?>(),
            It.IsAny<Expression<Func<Partner, bool>>?>(),
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>?>(),
            It.IsAny<Expression<Func<Partner, object>>?>(),
            It.IsAny<Expression<Func<Partner, object>>?>()))
        .Returns(PaginationResponse<Partner>.Create(returnList.AsQueryable()));
    }

    private void SetupMapper(IEnumerable<PartnerDTO> returnList)
    {
        _mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(returnList);
    }
}
