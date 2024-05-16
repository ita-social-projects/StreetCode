using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class CreateHistoricalContextTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public CreateHistoricalContextTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.HistoricalContextRepository.CreateAsync(new HistoricalContext()));
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            _mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>())).Returns(new HistoricalContextDTO());

            var handler = new CreateHistoricalContextHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreateHistoricalContextCommand(new HistoricalContextDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.True(result.IsSuccess));
        }
    }
}
