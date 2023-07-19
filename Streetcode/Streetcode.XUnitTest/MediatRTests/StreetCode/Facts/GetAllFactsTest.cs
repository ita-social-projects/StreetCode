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
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetAllFactsTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;  
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

    public GetAllFactsTest()
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
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        var handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsType<List<FactDto>>(result.ValueOrDefault)
        );
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_CountMatch()
    {
        //Arrange
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        var handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetListFacts().Count(), result.Value.Count())
        );
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExist()
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<Fact>,
              IIncludableQueryable<Fact, object>>>()))
              .ReturnsAsync(GetListFactsWithNotExistingId());

        _mockMapper
            .Setup(x => x
            .Map<IEnumerable<FactDto>>(It.IsAny<IEnumerable<Fact>>()))
            .Returns(GetListFactsDTOWithNotExistingId());

        var expectedError = "Cannot find any fact";

        var handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

        //Act
        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(
        Mock<IMapper> mockMapper,
        Mock<IRepositoryWrapper> mockRepository)
    {
        mockRepository.Setup(x => x.FactRepository
              .GetAllAsync(
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
                Id = 1
            },

            new Fact
            {
                Id = 2
            }
        };

        return facts.AsQueryable();
    }
    private static List<Fact>? GetListFactsWithNotExistingId()
    {
        return null;
    }
    private static List<FactDto>? GetListFactsDTOWithNotExistingId()
    {
        return null;
    }
    private static List<FactDto> GetListFactDTO()
    {
        var factsDTO = new List<FactDto>
        {
            new FactDto
            {
                Id = 1
            },

            new FactDto
            {
                Id = 2,
            }
        };

        return factsDTO;
    }
}
