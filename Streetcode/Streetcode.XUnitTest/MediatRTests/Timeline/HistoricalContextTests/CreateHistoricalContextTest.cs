﻿using AutoMapper;
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
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;

        public CreateHistoricalContextTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            this.mockRepo.Setup(repo => repo.HistoricalContextRepository.CreateAsync(new HistoricalContext()));
            this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            this.mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>())).Returns(new HistoricalContextDTO());

            var handler = new CreateHistoricalContextHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreateHistoricalContextCommand(new HistoricalContextDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.True(result.IsSuccess));
        }
    }
}
