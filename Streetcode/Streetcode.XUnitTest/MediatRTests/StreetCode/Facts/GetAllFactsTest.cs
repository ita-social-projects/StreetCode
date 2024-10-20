using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetAllFactsTest
{
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;
    private Mock<IRepositoryWrapper> mockRepository;
    private Mock<IMapper> mockMapper;

    public GetAllFactsTest()
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
        (this.mockMapper, this.mockRepository) = GetMapperAndRepo(this.mockMapper, this.mockRepository);

        var handler = new GetAllFactsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<FactDto>>(result.ValueOrDefault));
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        // Arrange
        (this.mockMapper, this.mockRepository) = GetMapperAndRepo(this.mockMapper, this.mockRepository);

        var handler = new GetAllFactsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetListFacts().Count(), result.Value.Count()));
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExist()
    {
        // Arrange
        this.mockRepository
            .Setup(x => x.FactRepository.GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetListFactsWithNotExistingId());

        this.mockMapper
            .Setup(x => x.Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactsDTOWithNotExistingId());

        var expectedError = "Cannot find any fact";
        this.mockLocalizerCannotFind.Setup(x => x["CannotFindAnyFact"])
           .Returns(new LocalizedString("CannotFindAnyFact", expectedError));

        var handler = new GetAllFactsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

        // Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(
        Mock<IMapper> mockMapper,
        Mock<IRepositoryWrapper> mockRepository)
    {
        mockRepository
            .Setup(x => x.FactRepository.GetAllAsync(
                It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetListFacts());

        mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactDTO());

        return (mockMapper, mockRepository);
    }

    private static IQueryable<Fact> GetListFacts()
    {
        var facts = new List<Fact>
        {
            new Fact
            {
                Id = 1,
            },
            new Fact
            {
                Id = 2,
            },
        };

        return facts.AsQueryable();
    }

    private static List<Fact> GetListFactsWithNotExistingId()
    {
        return new List<Fact>();
    }

    private static List<FactDto> GetListFactsDTOWithNotExistingId()
    {
        return new List<FactDto>();
    }

    private static List<FactDto> GetListFactDTO()
    {
        var factsDTO = new List<FactDto>
        {
            new FactDto
            {
                Id = 1,
            },
            new FactDto
            {
                Id = 2,
            },
        };

        return factsDTO;
    }
}
