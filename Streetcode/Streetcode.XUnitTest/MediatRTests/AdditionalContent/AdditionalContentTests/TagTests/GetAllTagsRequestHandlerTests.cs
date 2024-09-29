using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetAllTagsRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly List<Tag> tags = new List<Tag>()
        {
            new Tag
            {
                Id = 1,
                Title = "some title 1",
            },
            new Tag
            {
                Id = 2,
                Title = "some title 2",
            },
        };

        private readonly List<TagDTO> tagDTOs = new List<TagDTO>()
        {
            new TagDTO
            {
                Id = 1,
                Title = "some title 1",
            },
            new TagDTO
            {
                Id = 2,
                Title = "some title 2",
            },
        };

        public GetAllTagsRequestHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.tags);
            this.SetupMapper(this.tagDTOs);

            var handler = new GetAllTagsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value),
                () => Assert.True(result.Value.Count() == this.tags.Count));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupRepository(new List<Tag>());
            this.SetupMapper(new List<TagDTO>());

            var expectedError = $"Cannot find any tags";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnyTags"])
                .Returns(new LocalizedString("CannotFindAnyTags", expectedError));

            var handler = new GetAllTagsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<Tag> returnList)
        {
            this.mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<TagDTO> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
