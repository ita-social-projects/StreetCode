using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Helpers;
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
        this.SetupPaginatedRepository(GetPartnerList());
        this.SetupMapper(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PartnerDto>>(result.Value.Partners)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        this.SetupPaginatedRepository(GetPartnerList());
        this.SetupMapper(GetListPartnerDTO());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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
        this.SetupPaginatedRepository(GetPartnerList().Take(pageSize));
        this.SetupMapper(GetListPartnerDTO().Take(pageSize).ToList());

        var handler = new GetAllPartnersHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllPartnersQuery(page: 1, pageSize: pageSize), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<List<PartnerDto>>(result.Value.Partners),
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

    private static List<PartnerDto> GetListPartnerDTO()
    {
        var partnersDTO = new List<PartnerDto>
    {
        new PartnerDto
        {
            Id = 1,
        },
        new PartnerDto
        {
            Id = 2,
        },
        new PartnerDto
        {
            Id = 3,
        },
        new PartnerDto
        {
            Id = 4,
        },
        new PartnerDto
        {
            Id = 5,
        },
    };

        return partnersDTO;
    }

    private void SetupPaginatedRepository(IEnumerable<Partner> returnList)
    {
        this.mockRepository.Setup(repo => repo.PartnersRepository.GetAllPaginated(
            It.IsAny<ushort?>(),
            It.IsAny<ushort?>(),
            It.IsAny<Expression<Func<Partner, Partner>>?>(),
            It.IsAny<Expression<Func<Partner, bool>>?>(),
            It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>?>(),
            It.IsAny<Expression<Func<Partner, object>>?>(),
            It.IsAny<Expression<Func<Partner, object>>?>()))
        .Returns(PaginationResponse<Partner>.Create(returnList.AsQueryable()));
    }

    private void SetupMapper(IEnumerable<PartnerDto> returnList)
    {
        this.mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDto>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(returnList);
    }
}
