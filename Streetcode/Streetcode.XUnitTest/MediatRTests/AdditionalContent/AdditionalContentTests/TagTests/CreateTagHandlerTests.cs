using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class CreateTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;

        public CreateTagHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            this.mockRepo.Setup(repo => repo.TagRepository.CreateAsync(new Tag()));
            this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            this.mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>())).Returns(new TagDTO());

            var handler = new CreateTagHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreateTagQuery(new CreateTagDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<TagDTO>(result.Value),
               () => Assert.True(result.IsSuccess));
        }
    }
}
