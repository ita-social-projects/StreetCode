using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class CreateTagHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> _mockStringLocalizerFailedToValidate;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> _mockStringLocalizerFieldNames;

        public CreateTagHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            _mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.TagRepository.CreateAsync(new Tag()));
            _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>())).Returns(new TagDTO());


            var handler = new CreateTagHandler(
                _mockRepo.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockStringLocalizerFailedToValidate.Object,
                _mockStringLocalizerFieldNames.Object);

            // Act
            var result = await handler.Handle(new CreateTagQuery(new CreateTagDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
               () => Assert.IsType<TagDTO>(result.Value),
               () => Assert.True(result.IsSuccess));
        }
    }
}
