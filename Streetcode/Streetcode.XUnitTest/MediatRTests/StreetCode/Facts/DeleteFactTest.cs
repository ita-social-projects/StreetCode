using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class DeleteFactTest
{
    private Mock<IRepositoryWrapper> _repository;

    public DeleteFactTest()
    {
        _repository = new Mock<IRepositoryWrapper>();
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldDeleteSuccessfully(int id)
    {
        //Arrange
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        _repository.Setup(x => x.FactRepository
            .Delete(GetFact(id)));

        _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var handler = new DeleteFactHandler(_repository.Object);

        //Act
        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess)
        );
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldThrowExeption_IdNotExisting(int id)
    {
        //Arrange
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFactWithNotExistingId());

        _repository.Setup(x => x.FactRepository
        .Delete(GetFactWithNotExistingId()));

        var expectedError = $"Cannot find a fact with corresponding categoryId: {id}";

        //Act
        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    [Theory]
    [InlineData(2)]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int id)
    {
        //Arrange
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(GetFact(id));

        _repository.Setup(x => x.FactRepository
            .Delete(GetFact(id)));

        _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

        var expectedError = "Failed to delete a fact";

        //Act
        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static Fact GetFact(int id)
    {
        return new Fact
        {
            Id = id
        };
    }
    private static Fact? GetFactWithNotExistingId()
    {
        return null;
    }
}
