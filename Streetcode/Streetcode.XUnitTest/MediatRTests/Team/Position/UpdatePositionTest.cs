using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class UpdatePositionTest
{
    private readonly Mock<IRepositoryWrapper> mockRepo;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

    public UpdatePositionTest()
    {
        this.mockRepo = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        this.mockRepo.Setup(repo => repo.PositionRepository.Update(new Positions()));
        this.mockRepo.Setup(repo =>
                repo.PositionRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync((Expression<Func<Positions, bool>> expr, IIncludableQueryable<Positions, bool> include) =>
            {
                BinaryExpression eq = (BinaryExpression)expr.Body;
                MemberExpression member = (MemberExpression)eq.Left;
                return member.Member.Name == "Id" ? new Positions() : null;
            });

        this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        this.mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>())).Returns(new PositionDTO());

        var handler = new UpdateTeamPositionHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

        // Act
        var result = await handler.Handle(new UpdateTeamPositionCommand(new PositionDTO()), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.IsSuccess));
    }
}
