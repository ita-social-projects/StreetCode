using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class GetAllFactsTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetAllFactsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();

        _mockMapper = new Mock<IMapper>();
    }

    [Fact]
    public async Task GetAllFacts_ShouldReturnSuccessfullyType()
    {
        //Arrange
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        //Act
        var handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.IsType<List<FactDTO>>(result.ValueOrDefault);
    }

    [Fact]
    public async Task GetAllFacts_ShouldReturnSuccessfullyCountMatch()
    {
        //Arrange
        (_mockMapper, _mockRepository) = GetMapperAndRepo(_mockMapper, _mockRepository);

        //Act
        var handler = new GetAllFactsHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetAllFactsQuery(), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetListFacts().Count(), result.Value.Count())
        );
    }

    private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(Mock<IMapper> injectedMapper, Mock<IRepositoryWrapper> injectedReppo)
    {
        injectedReppo.Setup(x => x.FactRepository
              .GetAllAsync(
                  It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                    It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
              IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
              .ReturnsAsync(GetListFacts());

        injectedMapper
            .Setup(x => x
            .Map<IEnumerable<FactDTO>>(It.IsAny<IEnumerable<DAL.Entities.Streetcode.TextContent.Fact>>()))
            .Returns(GetListFactDTO());

        return (injectedMapper, injectedReppo);
    }
    private static IQueryable<DAL.Entities.Streetcode.TextContent.Fact> GetListFacts()
    {
        var facts = new List<DAL.Entities.Streetcode.TextContent.Fact>
        {
            new DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                Image = null,
                FactContent = "Навесні 1838-го Карл Брюллов..."
            },

            new DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = 2,
                Title = "Перший Кобзар",
                ImageId = 5,
                Image = null,
                FactContent = "Ознайомившись випадково з рукописними творами"
            }
        };

        return facts.AsQueryable();
    }

    private static List<FactDTO> GetListFactDTO()
    {
        var factsDTO = new List<FactDTO>
        {
            new FactDTO
            {
                Id = 1,
                Title = "Викуп з кріпацтва",
                ImageId = null,
                FactContent = "Навесні 1838-го Карл Брюллов..."
            },

            new FactDTO
            {
                Id = 2,
                Title = "Перший Кобзар",
                ImageId = 5,
                FactContent = "Ознайомившись випадково з рукописними творами"
            }
        };

        return factsDTO;
    }
}
