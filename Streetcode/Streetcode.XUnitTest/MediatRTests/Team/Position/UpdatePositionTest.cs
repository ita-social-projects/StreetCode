using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.Update;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class UpdatePositionTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public UpdatePositionTest()
    {
        this._mockRepo = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        this._mockRepo.Setup(repo => repo.PositionRepository.Update(new Positions()));
        this._mockRepo.Setup(repo =>
                repo.PositionRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Positions, bool>>>(), default))
            .ReturnsAsync((Expression<Func<Positions, bool>> expr, IIncludableQueryable<Positions, bool> include) =>
            {
                BinaryExpression eq = (BinaryExpression)expr.Body;
                MemberExpression member = (MemberExpression)eq.Left;
                return member.Member.Name == "Id" ? new Positions() : null;
            });

        this._mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);
        this._mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>())).Returns(new PositionDTO());

        var handler = new UpdateTeamPositionHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object);

        // Act
        var result = await handler.Handle(new UpdateTeamPositionCommand(new PositionDTO()), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.IsType<PositionDTO>(result.Value),
            () => Assert.True(result.IsSuccess));
    }
}
