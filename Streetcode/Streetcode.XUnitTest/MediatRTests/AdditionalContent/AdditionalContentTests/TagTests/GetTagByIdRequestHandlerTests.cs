using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagByIdRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetTagByIdRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        private const int _id = 1;

        private readonly Tag tag = new Tag
        {
            Id = 1,
            Title = "some title 1"
        };
        private readonly TagDTO tagDTO = new TagDTO
        {
            Id = 1,
            Title = "some title 1"
        };

        [Fact]
        public async Task GetTagById_ReturnElement()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tag);

            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns(tagDTO);

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<TagDTO>(result.Value);

            Assert.True(result.Value.Id.Equals(_id));
        }

        [Fact]
        public async Task GetTagById_ReturnsNoElements()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.TagRepository.GetFirstOrDefaultAsync(
            It.IsAny<Expression<Func<Tag, bool>>>(),
            It.IsAny<Func<IQueryable<Tag>,
            IIncludableQueryable<Tag, object>>>()))
            .ReturnsAsync(new Tag()); //returns default

            _mockMapper.Setup(x => x.Map<TagDTO>(It.IsAny<Tag>()))
                .Returns(new TagDTO());

            var handler = new GetTagByIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.NotNull(result);

            Assert.NotEqual(result.Value.Id, _id);
        }
    }
}
