using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

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
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
                IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
            .ReturnsAsync(new DAL.Entities.Streetcode.TextContent.Fact
            {
                Id = id
            });

        _repository.Setup(x => x.FactRepository
            .Delete(new DAL.Entities.Streetcode.TextContent.Fact()
            {
                Id = id
            }));

        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        Assert.NotNull(result);
    }

    [Theory]
    [InlineData(2)]
    public async Task DeleteFact_ShouldReturnNull(int id)
    {
        _repository.Setup(x => x.FactRepository
        .GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<DAL.Entities.Streetcode.TextContent.Fact, bool>>>(),
                It.IsAny<Func<IQueryable<DAL.Entities.Streetcode.TextContent.Fact>,
                IIncludableQueryable<DAL.Entities.Streetcode.TextContent.Fact, object>>>()))
            .ReturnsAsync((DAL.Entities.Streetcode.TextContent.Fact)null);

        _repository.Setup(x => x.FactRepository
            .Delete(new DAL.Entities.Streetcode.TextContent.Fact()
            {
                Id = id
            }));

        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        Assert.Equal($"Cannot find a fact with corresponding categoryId: {id}", result.Errors.First().Message);
    }
}
