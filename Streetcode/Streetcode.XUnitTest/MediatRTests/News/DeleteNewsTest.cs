using Moq;
using Streetcode.BLL.DTO.News;
using Streetcode.BLL.MediatR.Newss.Delete;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.News;

public class DeleteNewsTest
{
    private Mock<IRepositoryWrapper> _mockRepository;

    public DeleteNewsTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
    }

    [Fact]
    public async Task ShouldDeleteSuccessfully()
    {
        //Arrange
        var testNews = GetNews();


        _mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(), null))
            .ReturnsAsync(testNews);
        _mockRepository.Setup(x => x.SaveChangesAsync())
            .ReturnsAsync(1);


        var handler = new DeleteNewsHandler(_mockRepository.Object);

        //Act
        var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

        //Assert
        Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess)
        );

        _mockRepository.Verify(x => x.NewsRepository.Delete(It.Is<DAL.Entities.News.News>(x => x.Id == testNews.Id)), Times.Once);
        _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ShouldThrowExeption_IdNotExisting()
    {
        //Arrange
        var testNews = GetNews();
        var expectedError = $"No news found by entered Id - {testNews.Id}";

        _mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(), null))
            .ReturnsAsync((DAL.Entities.News.News)null);


        //Act
        var handler = new DeleteNewsHandler(_mockRepository.Object);

        var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);

        _mockRepository.Verify(x => x.NewsRepository.Delete(It.IsAny<DAL.Entities.News.News>()), Times.Never);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        //Arrange
        var testNews = GetNews();
        var expectedError = "Failed to delete news";

        _mockRepository.Setup(x => x.NewsRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<DAL.Entities.News.News, bool>>>(), null))
            .ReturnsAsync(testNews);
        _mockRepository.Setup(x => x.SaveChanges())
            .Throws(new Exception(expectedError));

        //Act
        var handler = new DeleteNewsHandler(_mockRepository.Object);

        var result = await handler.Handle(new DeleteNewsCommand(testNews.Id), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static DAL.Entities.News.News GetNews()
    {
        return new DAL.Entities.News.News()
        {
            Id = 1
        };
    }

    private static NewsDTO GetNewsDTO()
    {
        return new NewsDTO();
    }

    private static NewsDTO? GetNewsWithNotExistingId()
    {
        return null;
    }

}
