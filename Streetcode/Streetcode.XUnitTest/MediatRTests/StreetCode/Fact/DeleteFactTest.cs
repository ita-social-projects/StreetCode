using AutoMapper;
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
    public async Task DeleteFact_ShouldDeleteSuccessfully(int id)
    {
        //Arrange
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync(new Fact
            {
                Id = id
            });

        _repository.Setup(x => x.FactRepository
            .Delete(new Fact()
            {
                Id = id
            }));

        //Act
        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        //Assert
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(2)]
    public async Task DeleteFact_ShouldReturnNull(int id)
    {
        //Arrange
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<Fact, bool>>>(),
                It.IsAny<Func<IQueryable<Fact>,
                IIncludableQueryable<Fact, object>>>()))
            .ReturnsAsync((Fact)null);

        _repository.Setup(x => x.FactRepository
            .Delete(new Fact()
            {
                Id = id
            }));

        //Act
        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        //Assert
        Assert.Equal($"Cannot find a fact with corresponding categoryId: {id}", result.Errors.First().Message);
    }
}
