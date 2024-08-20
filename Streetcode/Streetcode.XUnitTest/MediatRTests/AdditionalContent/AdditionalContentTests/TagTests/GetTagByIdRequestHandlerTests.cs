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
        private const int _id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly Tag tag = new Tag
        {
            Id = _id,
            Title = "some title 1",
        };

        private readonly TagDTO tagDTO = new TagDTO
        {
            Id = _id,
            Title = "some title 1",
        };

        public GetTagByIdRequestHandlerTests()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            this.SetupRepository(this.tag);
            this.SetupMapper(this.tagDTO);

            var handler = new GetTagByIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(_id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            this.SetupRepository(new Tag());
            this.SetupMapper(new TagDTO());

            var handler = new GetTagByIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(_id)));
        }

        private void SetupRepository(Tag tag)
        {
            this._mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tag);
        }

        private void SetupMapper(TagDTO tagDTO)
        {
            this._mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns(tagDTO);
        }
    }
}
