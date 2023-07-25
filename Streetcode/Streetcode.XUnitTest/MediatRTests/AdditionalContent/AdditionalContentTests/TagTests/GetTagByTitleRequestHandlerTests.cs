using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetTagByTitle;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagByTitleRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetTagByTitleRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private static string _title = "test_title";

        private readonly Tag tag = new Tag
        {
            Id = 1,
            Title = _title
        };
        private readonly TagDTO tagDTO = new TagDTO
        {
            Id = 1,
            Title = _title
        };

        async Task SetupRepository(Tag tag)
        {
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tag);
        }
        async Task SetupMapper(TagDTO tagDTO)
        {
            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns(tagDTO);
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            //Arrange
            await SetupRepository(tag);
            await SetupMapper(tagDTO);

            var handler = new GetTagByTitleHandler(_mockRepo.Object, _mockMapper.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetTagByTitleQuery(_title), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.Equal(result.Value.Title, _title));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            //Arrange
            await SetupRepository(new Tag());
            await SetupMapper(new TagDTO());

            var handler = new GetTagByTitleHandler(_mockRepo.Object, _mockMapper.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetTagByTitleQuery(_title), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.Null(result.Value.Title));
        }
    }
}
