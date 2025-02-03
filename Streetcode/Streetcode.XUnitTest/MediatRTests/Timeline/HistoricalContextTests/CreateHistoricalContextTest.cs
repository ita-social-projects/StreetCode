using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.Create;
using Streetcode.BLL.SharedResource;
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
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockLocalizerValidation;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockLocalizerFieldNames;

        public CreateHistoricalContextTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerValidation = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            this.mockLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            this.mockRepo.Setup(repo => repo.HistoricalContextRepository.CreateAsync(new HistoricalContext()));
            this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            this.mockMapper.Setup(x => x.Map<HistoricalContextDto>(It.IsAny<HistoricalContext>())).Returns(new HistoricalContextDto());

            var handler = new CreateHistoricalContextHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object, this.mockLocalizerValidation.Object, this.mockLocalizerFieldNames.Object);

            // Act
            var result = await handler.Handle(new CreateHistoricalContextCommand(new HistoricalContextDto()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDto>(result.Value),
                () => Assert.True(result.IsSuccess));
        }
    }
}
