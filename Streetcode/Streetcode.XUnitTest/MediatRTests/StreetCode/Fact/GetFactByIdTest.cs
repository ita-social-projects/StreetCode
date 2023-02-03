using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

public class GetFactByIdTest
{
    private Mock<IRepositoryWrapper> _mockRepository;
    private Mock<IMapper> _mockMapper;

    public GetFactByIdTest() {
        _mockMapper = new Mock<IMapper>();
        _mockRepository = new Mock<IRepositoryWrapper>();
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
                IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
            .ReturnsAsync(new DAL.Entities.Streetcode.TextContent.Fact()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<DAL.Entities.Streetcode.TextContent.Fact>()))
            .Returns(new FactDTO { Id = id});

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(result.Value.Id, id);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyNotExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
                IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
            .ReturnsAsync((DAL.Entities.Streetcode.TextContent.Fact)null);

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<DAL.Entities.Streetcode.TextContent.Fact>()))
            .Returns((DAL.Entities.Streetcode.TextContent.Fact fact) =>
            {
                return new FactDTO { Id = fact.Id };
            });

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailed);
        Assert.Equal($"Cannot find a fact with corresponding id: {id}", result.Errors.First().Message);
    }

    [Theory]
    [InlineData(1)]
    public async Task GetById_ShouldReturnSuccessfullyCorrectType(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
                IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
            .ReturnsAsync(new DAL.Entities.Streetcode.TextContent.Fact()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<DAL.Entities.Streetcode.TextContent.Fact>()))
            .Returns(new FactDTO {
                Id = id
            });

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result.ValueOrDefault);
        Assert.IsType<FactDTO>(result.ValueOrDefault);
    }
}
