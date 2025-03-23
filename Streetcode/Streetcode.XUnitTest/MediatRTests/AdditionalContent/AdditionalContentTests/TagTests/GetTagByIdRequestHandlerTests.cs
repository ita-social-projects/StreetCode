using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagByIdRequestHandlerTests
    {
        private const int Id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly Tag _tag = new Tag
        {
            Id = Id,
            Title = "some title 1",
        };

        private readonly TagDTO _tagDto = new TagDTO
        {
            Id = Id,
            Title = "some title 1",
        };

        public GetTagByIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            SetupRepository(_tag);
            SetupMapper(_tagDto);

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByIdQuery(Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(Id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            SetupRepository(new Tag());
            SetupMapper(new TagDTO());

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByIdQuery(Id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(Id)));
        }

        private void SetupRepository(Tag tag)
        {
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tag);
        }

        private void SetupMapper(TagDTO tagDto)
        {
            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns(tagDto);
        }
    }
}
