using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Partners;

public class GetParnerByStreetcodeIdTest
{
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public GetParnerByStreetcodeIdTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_ExistingId()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        this.mockRepository.Setup(x => x.StreetcodeRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                null))
            .ReturnsAsync(testStreetcodeContent);

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDto>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CorrectType()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        this.mockRepository.Setup(x => x.StreetcodeRepository
            .GetSingleOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                null))
            .ReturnsAsync(testStreetcodeContent);

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(GetPartnerList());

        this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<PartnerDto>>(It.IsAny<IEnumerable<Partner>>()))
            .Returns(GetPartnerDTOList());

        var handler = new GetPartnersByStreetcodeIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.IsType<List<PartnerDto>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_EmptyList()
    {
        // Arrange
        var testStreetcodeContent = GetStreetcodeList()[0];

        this.mockRepository.Setup(x => x.StreetcodeRepository
           .GetSingleOrDefaultAsync(
               It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
               null))
           .ReturnsAsync(testStreetcodeContent);

        this.mockRepository.Setup(x => x.PartnersRepository
            .GetAllAsync(
                It.IsAny<Expression<Func<Partner, bool>>>(),
                It.IsAny<Func<IQueryable<Partner>,
                IIncludableQueryable<Partner, object>>>()))
            .ReturnsAsync(new List<Partner>());

        var handler = new GetPartnersByStreetcodeIdHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetPartnersByStreetcodeIdQuery(testStreetcodeContent.Id), CancellationToken.None);

        // Asset
        Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<PartnerDto>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<PartnerDto>>(result.Value),
                () => Assert.Empty(result.Value));
    }

    private static IEnumerable<Partner> GetPartnerList()
    {
        var partners = new List<Partner>
        {
            new Partner
            {
                Id = 1,
                Streetcodes = GetStreetcodeList(),
            },
        };

        return partners;
    }

    private static List<StreetcodeContent> GetStreetcodeList()
    {
        var streetCodes = new List<StreetcodeContent>
        {
            new StreetcodeContent
            {
                Id = 1,
            },
        };

        return streetCodes;
    }

    private static List<PartnerDto> GetPartnerDTOList()
    {
        var partners = new List<PartnerDto>
        {
            new PartnerDto
            {
                Id = 1,
            },
        };

        return partners;
    }
}
