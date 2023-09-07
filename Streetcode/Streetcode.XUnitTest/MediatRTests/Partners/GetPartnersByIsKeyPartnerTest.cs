using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.MediatR.Partners.GetByIsKeyPartner;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetPartnersByIsKeyPartnerTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
    
    public GetPartnersByIsKeyPartnerTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        var testPartner = GetPartnerList().First();
        this._mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this._mockMapper
            .Setup(x => x.Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDto());

        var handler = new GetPartnersByIsKeyPartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

        var result = await handler.Handle(new GetPartnersByIsKeyPartnerQuery(testPartner.IsKeyPartner), CancellationToken.None);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<PartnerDTO>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        var testPartner = GetPartnerList().First();
        this._mockRepository.Setup(x => x.PartnersRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Partner>, IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this._mockMapper
            .Setup(x => x
                .Map<IEnumerable<PartnerDTO>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetListPartnerDto());

        var handler = new GetPartnersByIsKeyPartnerHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);
        var result = await handler.Handle(new GetPartnersByIsKeyPartnerQuery(testPartner.IsKeyPartner), CancellationToken.None);

        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetPartnerList().Count(), result.Value.Count()));
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1,
                IsKeyPartner = true,
            },

            new Partner
            {
                Id = 2,
                IsKeyPartner = false,
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
                IsKeyPartner = true,
            },

            new PartnerDTO
            {
                Id = 2,
                IsKeyPartner = false,
            },
        };

        return partnersDto;
    }
}