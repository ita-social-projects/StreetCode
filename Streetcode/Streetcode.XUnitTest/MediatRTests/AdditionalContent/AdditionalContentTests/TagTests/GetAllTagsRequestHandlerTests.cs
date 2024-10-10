using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetAllTagsRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetAllTagsRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer  = new Mock<IStringLocalizer<CannotFindSharedResource>>();
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

        private void SetupPaginatedRepository(IEnumerable<Tag> returnList)
        {
            _mockRepo.Setup(repo => repo.TagRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<Tag, Tag>>?>(),
                It.IsAny<Expression<Func<Tag, bool>>?>(),
                It.IsAny<Func<IQueryable<Tag>, IIncludableQueryable<Tag, object>>?>(),
                It.IsAny<Expression<Func<Tag, object>>?>(),
                It.IsAny<Expression<Func<Tag, object>>?>()))
            .Returns(PaginationResponse<Tag>.Create(returnList.AsQueryable()));
        }

        private async Task SetupMapper(List<TagDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            SetupPaginatedRepository(tags);
            await SetupMapper(tagDTOs);

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value.Tags),
                () => Assert.True(result.Value.Tags.Count() == tags.Count));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            SetupPaginatedRepository(new List<Tag>());
            await SetupMapper(new List<TagDTO>());

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value.Tags),
                () => Assert.Empty(result.Value.Tags));
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            //Arrange
            ushort pageSize = 3;
            SetupPaginatedRepository(tags.Take(pageSize));
            await SetupMapper(tagDTOs.Take(pageSize).ToList());

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value.Tags),
                () => Assert.Equal(pageSize, result.Value.Tags.Count()));
        }
    }
}
