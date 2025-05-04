using System.Linq.Expressions;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Delete;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class DeleteTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<ILoggerService> _mockLogger;

        public DeleteTagHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.TagRepository.Delete(new Tag()));
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Tag, bool>>>(), default)).ReturnsAsync(new Tag());

            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteTagHandler(_mockRepo.Object, _mockLogger.Object);

            // Act
            var result = await handler.Handle(new DeleteTagCommand(1), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<int>(result.Value),
               () => Assert.True(result.IsSuccess));
        }
    }
}
