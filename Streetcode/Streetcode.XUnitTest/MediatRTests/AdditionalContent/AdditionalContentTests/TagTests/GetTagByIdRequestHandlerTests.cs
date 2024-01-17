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
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetTagByIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private const int _id = 1;

        private readonly Tag tag = new Tag
        {
            Id = _id,
            Title = "some title 1"
        };
        private readonly TagDTO tagDTO = new TagDTO
        {
            Id = _id,
            Title = "some title 1"
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

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(_id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            //Arrange
            await SetupRepository(new Tag());
            await SetupMapper(new TagDTO());

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<TagDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(_id)));
        }
    }
}
