using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Mapping.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Fact;

public class DeleteFactTest
{
    private Mock<IRepositoryWrapper> _repository;
    private Mock<IMapper> _mockMapper;

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

        _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        var handler = new DeleteFactHandler(_repository.Object);

        var result = await handler.Handle(new DeleteFactCommand(id), CancellationToken.None);

        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
    }
}
