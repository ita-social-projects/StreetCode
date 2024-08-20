using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests;

public class UpdateHistoricalContextTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public UpdateHistoricalContextTest()
    {
        this._mockRepo = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        this._mockRepo.Setup(repo => repo.HistoricalContextRepository.Update(new HistoricalContext()));
        this._mockRepo.Setup(repo =>
                repo.HistoricalContextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync((Expression<Func<HistoricalContext, bool>> expr, IIncludableQueryable<HistoricalContext, bool> include) =>
            {
                BinaryExpression eq = (BinaryExpression)expr.Body;
                MemberExpression member = (MemberExpression)eq.Left;
                return member.Member.Name == "Id" ? new HistoricalContext() : null;
            });

        this._mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        this._mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>())).Returns(new HistoricalContextDTO());

        var handler = new UpdateHistoricalContextHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new UpdateHistoricalContextCommand(new HistoricalContextDTO()), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.True(result.IsSuccess));
    }
}