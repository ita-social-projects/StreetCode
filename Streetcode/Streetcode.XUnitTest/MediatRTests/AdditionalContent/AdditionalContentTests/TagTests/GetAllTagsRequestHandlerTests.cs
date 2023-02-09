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

        async Task SetupRepository(List<Tag> returnList)
        {
            _mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(returnList);
        }
        async Task SetupMapper(List<TagDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            await SetupRepository(tags);
            await SetupMapper(tagDTOs);

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value),
                () => Assert.True(result.Value.Count() == tags.Count));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            await SetupRepository(new List<Tag>());
            await SetupMapper(new List<TagDTO>());

            var handler = new GetAllTagsHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllTagsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }
    }
}
