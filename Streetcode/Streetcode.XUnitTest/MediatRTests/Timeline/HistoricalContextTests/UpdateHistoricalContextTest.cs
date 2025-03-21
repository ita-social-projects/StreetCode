using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests;

public class UpdateHistoricalContextTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToValidateLocalizer _mockLocalizerValidation;
    private readonly MockFieldNamesLocalizer _mockLocalizerFieldNames;
    private readonly MockCannotFindLocalizer _mockLocalizerCannotFind;

    public UpdateHistoricalContextTest()
    {
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _mockLocalizerValidation = new MockFailedToValidateLocalizer();
        _mockLocalizerFieldNames = new MockFieldNamesLocalizer();
        _mockLocalizerCannotFind = new MockCannotFindLocalizer();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        SetupRepo(new HistoricalContext(), null, new HistoricalContext(), true);
        SetupMapper(new HistoricalContextDTO());

        var handler = new UpdateHistoricalContextHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind, _mockLocalizerValidation, _mockLocalizerFieldNames);

        // Act
        var result = await handler.Handle(new UpdateHistoricalContextCommand(new HistoricalContextDTO()), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDTO>(result.Value),
            () => Assert.True(result.IsSuccess));
    }

    [Fact]
    public async Task ShouldReturnBadRequest_ChangesNotSaved()
    {
        // Arrange
        SetupRepo(new HistoricalContext(), null, new HistoricalContext(), false);

        var handler = new UpdateHistoricalContextHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind, _mockLocalizerValidation, _mockLocalizerFieldNames);

        // Act
        var result = await handler.Handle(new UpdateHistoricalContextCommand(new HistoricalContextDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
    }

    private void SetupRepo(HistoricalContext? getValue, HistoricalContext? getRepeatValue, HistoricalContext? updateValue, bool saveValue)
    {
        _mockRepo.Setup(repo => repo.HistoricalContextRepository.Update(updateValue));
        _mockRepo.Setup(repo =>
                repo.HistoricalContextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync((Expression<Func<HistoricalContext, bool>> expr, IIncludableQueryable<HistoricalContext, bool> include) =>
            {
                BinaryExpression eq = (BinaryExpression)expr.Body;
                MemberExpression member = (MemberExpression)eq.Left;
                return member.Member.Name == "Id" ? getValue : getRepeatValue;
            });
        if (saveValue)
        {
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        }
        else
        {
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).Throws(new Exception());
        }
    }

    private void SetupMapper(HistoricalContextDTO getValue)
    {
        _mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>())).Returns(getValue);
    }
}