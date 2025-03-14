﻿using System.Linq.Expressions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class DeleteHistoricalContextTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public DeleteHistoricalContextTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldDeleteSuccessfully()
        {
            // Arrange
            var testContexts = DeleteContext();
            this.SetupMockRepositoryGetFirstOrDefault(testContexts);
            this.SetupMockRepositorySaveChangesReturns(1);

            var handler = new DeleteHistoricalContextHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new DeleteHistoricalContextCommand(testContexts.Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess));

            _mockRepository.Verify(
                x => x.HistoricalContextRepository.Delete(It.Is<HistoricalContext>(x => x.Id == testContexts.Id)),
                Times.Once);
            _mockRepository.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ShouldThrowException_IdNotExisting()
        {
            // Arrange
            var testContexts = DeleteContext();
            var expectedError = "CannotFindHistoricalContextWithCorrespondingId";
            _mockLocalizer.Setup(x => x[expectedError, It.IsAny<object[]>()]).Returns(
                new LocalizedString(expectedError, expectedError));
            this.SetupMockRepositoryGetFirstOrDefault(null);

            var handler = new DeleteHistoricalContextHandler(_mockRepository.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new DeleteHistoricalContextCommand(testContexts.Id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
            _mockRepository.Verify(x => x.HistoricalContextRepository.Delete(It.IsAny<HistoricalContext>()), Times.Never);
        }

        private static HistoricalContext DeleteContext()
        {
            return new HistoricalContext
            {
                Id = 1,
            };
        }

        private void SetupMockRepositoryGetFirstOrDefault(HistoricalContext? context)
        {
            _mockRepository
                .Setup(x => x.HistoricalContextRepository
                    .GetFirstOrDefaultAsync(It.IsAny<Expression<Func<HistoricalContext, bool>>>(), null))
                .ReturnsAsync(context);
        }

        private void SetupMockRepositorySaveChangesReturns(int number)
        {
            _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(number);
        }
    }
}