using Xunit;
using Moq;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetAllTagsRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllTagsRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>(); 
        }

        private readonly List<Tag> tags = new List<Tag>()
        {
            new Tag
            {
                Id = 1,
                Title = "some title 1"
            },
            new Tag
            {
                Id = 2,
                Title = "some title 2"
            }
        };
        private readonly List<TagDTO> tagDTOs = new List<TagDTO>()
        {
            new TagDTO
            {
                Id = 1,
                Title = "some title 1"
            },
            new TagDTO
            {
                Id = 2,
                Title = "some title 2"
            }
        };

        [Fact]
        public async Task GetAllTags_ReturnsList()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(value: tags);

            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(tagDTOs);

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.IsType<List<TagDTO>>(result.Value);

            Assert.True(result.Value.Count() == tags.Count);
        }

        [Fact]
        public async Task GetAllTags_ReturnsNoElements()
        {
            _mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new List<Tag>());

            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(new List<TagDTO>());

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.Empty(result.Value);
        }
    }
}
