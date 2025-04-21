using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Serilog;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.BlobStorage;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Helpers;
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

        public GetAllTagsRequestHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private readonly List<Tag> tags = new List<Tag>()
        {
            new Tag { Id = 1, Title = "some title 1" },
            new Tag { Id = 2, Title = "some title 2" },
            new Tag { Id = 3, Title = "some title 3" },
            new Tag { Id = 4, Title = "some title 4" },
            new Tag { Id = 5, Title = "some title 5" },
        };

        private readonly List<TagDTO> tagDTOs = new List<TagDTO>()
        {
            new TagDTO { Id = 1, Title = "some title 1" },
            new TagDTO { Id = 2, Title = "some title 2" },
            new TagDTO { Id = 3, Title = "some title 3" },
            new TagDTO { Id = 4, Title = "some title 4" },
            new TagDTO { Id = 5, Title = "some title 5" },
        };

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupGetAllAsync(this.tags);
            this.SetupMapper(this.tagDTOs);

            var handler = new GetAllTagsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTagsQuery(UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value.Tags),
                () => Assert.True(result.Value.Tags.Count() == this.tags.Count));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupGetAllAsync(new List<Tag>());
            this.SetupMapper(new List<TagDTO>());

            var expectedError = "Cannot find any tags";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnyTags"])
                .Returns(new LocalizedString("CannotFindAnyTags", expectedError));

            var handler = new GetAllTagsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTagsQuery(UserRole.User), CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed, "Expected result to be a failure.");
            Assert.Equal(expectedError, result.Errors.First().Message);
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            // Arrange
            ushort pageSize = 3;
            this.SetupGetAllAsync(this.tags.Take(pageSize));
            this.SetupMapper(this.tagDTOs.Take(pageSize).ToList());

            var handler = new GetAllTagsHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTagsQuery(UserRole.User, page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value.Tags),
                () => Assert.Equal(pageSize, result.Value.Tags.Count()));
        }

        private void SetupGetAllAsync(IEnumerable<Tag> returnList)
        {
            this.mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>, IIncludableQueryable<Tag, object>>>()
            ))
            .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<TagDTO> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<Tag>>()))
                .Returns(returnList);
        }
    }
}