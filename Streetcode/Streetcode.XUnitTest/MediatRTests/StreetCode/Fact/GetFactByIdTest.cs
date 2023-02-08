using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetById;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

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
    public async Task ShouldReturnSuccessfully_ExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(new Fact()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<Fact>()))
            .Returns(new FactDTO { Id = id});

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.Equal(result.Value.Id, id)
        );
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_NotExistingId(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync((Fact)null);

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<Fact>()))
            .Returns((Fact fact) =>
            {
                return new FactDTO { Id = fact.Id };
            });

        var expectedError = $"Cannot find any fact with corresponding id: {id}";

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () =>Assert.True(result.IsFailed),
            () => Assert.Equal(expectedError, result.Errors.First().Message)
        );
    }

    [Theory]
    [InlineData(1)]
    public async Task ShouldReturnSuccessfully_CorrectType(int id)
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository
            .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(new Fact()
            {
                Id = id
            });

        _mockMapper
            .Setup(x => x
            .Map<FactDTO>(It.IsAny<Fact>()))
            .Returns(new FactDTO {
                Id = id
            });

        //Act
        var handler = new GetFactByIdHandler(_mockRepository.Object, _mockMapper.Object);

        var result = await handler.Handle(new GetFactByIdQuery(id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result.ValueOrDefault),
            () => Assert.IsType<FactDTO>(result.ValueOrDefault)
        );
    }
}
