using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.StreetcodeTest.TextTest
{
    public class GetAllTextsTest
    { 
    [Fact]
    public async Task GetAlltextsSuccess()
    {
        var repository = new Mock<IRepositoryWrapper>();
        var mockMapper = new Mock<IMapper>();

        var testTextsList = new List<Text>()
        {
            new Text() { Id = 1 }
        };

        var testTextslistDTO = new List<TextDTO>()
        {
            new TextDTO() { Id = 1 }
        };

        var testTexts = new Text() { Id = 1 };

        repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(), It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync(testTexts);

        repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync(testTextsList);

        mockMapper.Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>())).Returns(testTextslistDTO);

        var handler = new GetAllTextsHandler(repository.Object, mockMapper.Object);

        var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Value);
    }

    [Fact]
    public async Task GetAllTextsError()
    {
        var repository = new Mock<IRepositoryWrapper>();
        var mockMapper = new Mock<IMapper>();

        repository.Setup(repo => repo.TextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Text, bool>>>(),
            It.IsAny<Func<IQueryable<Text>, IIncludableQueryable<Text, Text>>?>()))
            .ReturnsAsync((Text)null);

        repository.Setup(repo => repo.TextRepository.GetAllAsync(null, null)).ReturnsAsync((List<Text>)null);

        mockMapper.Setup(x => x.Map<IEnumerable<TextDTO>>(It.IsAny<IEnumerable<object>>())).Returns(new List<TextDTO>() { new TextDTO() { Id = 1 } });

        var handler = new GetAllTextsHandler(repository.Object, mockMapper.Object);

        var result = await handler.Handle(new GetAllTextsQuery(), CancellationToken.None);

        Assert.NotNull(result);
    }
}

}


