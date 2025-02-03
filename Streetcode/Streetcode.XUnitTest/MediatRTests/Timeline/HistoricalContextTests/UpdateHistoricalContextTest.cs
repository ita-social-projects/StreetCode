using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests;

public class UpdateHistoricalContextTest
{
    private readonly Mock<IRepositoryWrapper> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockLocalizerValidation;
    private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockLocalizerFieldNames;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

    public UpdateHistoricalContextTest()
    {
        this.mockRepo = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerValidation = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
        this.mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        this.mockRepo.Setup(repo => repo.HistoricalContextRepository.Update(new HistoricalContext()));
        this.mockRepo.Setup(repo =>
                repo.HistoricalContextRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), default))
            .ReturnsAsync((Expression<Func<HistoricalContext, bool>> expr, IIncludableQueryable<HistoricalContext, bool> include) =>
            {
                BinaryExpression eq = (BinaryExpression)expr.Body;
                MemberExpression member = (MemberExpression)eq.Left;
                return member.Member.Name == "Id" ? new HistoricalContext() : null;
            });

        this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        this.mockMapper.Setup(x => x.Map<HistoricalContextDto>(It.IsAny<HistoricalContext>())).Returns(new HistoricalContextDto());

        var handler = new UpdateHistoricalContextHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object, this.mockLocalizerValidation.Object, this.mockLocalizerFieldNames.Object);

        // Act
        var result = await handler.Handle(new UpdateHistoricalContextCommand(new HistoricalContextDto()), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<HistoricalContextDto>(result.Value),
            () => Assert.True(result.IsSuccess));
    }
}