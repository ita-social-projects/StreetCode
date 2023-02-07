using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.TagTests
{
    public class GetTagsByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetTagsByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        private const int _streetcode_id = 1;

        private readonly List<Tag> tags = new List<Tag>()
        {
            new Tag
            {
                Id = 1,
                Streetcodes = new List<StreetcodeContent> {
                    new StreetcodeContent {
                        Id = 1
                    }
                }
            },
            new Tag
            {
                Id = 2,
                Streetcodes = new List<StreetcodeContent> {
                    new StreetcodeContent {
                        Id = 1
                    }
                }
            }
        };
        private readonly List<TagDTO> tagDTOs = new List<TagDTO>()  
        {
            new TagDTO
            {
                Id = 1,
                Streetcodes = new List<StreetcodeDTO> {
                    new EventStreetcodeDTO {
                        Id = 1
                    }
                }
            },
            new TagDTO
            {
                Id = 2,
                Streetcodes = new List<StreetcodeDTO> {
                    new EventStreetcodeDTO {
                        Id = 1
                    }
                }
            }
        };

        [Fact]
        public async Task GetSTagsByStreetcodeId_ReturnsList()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(tags);

            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(tagDTOs);

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);
            
            //Assert
            Assert.NotNull(result.Value);

            Assert.IsType<List<TagDTO>>(result.Value);

            Assert.True(result.Value.All(x =>
                x.Streetcodes.All(y => y.Id == _streetcode_id)));
        }

        [Fact]
        public async Task GetSubtitlesByStreetcodeId_ReturnsNoElements()
        {
            //Arrange
            _mockRepo.Setup(repo => repo.TagRepository.GetAllAsync(
                It.IsAny<Expression<Func<Tag, bool>>>(),
                It.IsAny<Func<IQueryable<Tag>,
                IIncludableQueryable<Tag, object>>>()))
                .ReturnsAsync(new List<Tag>()); //default

            _mockMapper.Setup(x => x.Map<IEnumerable<TagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(new List<TagDTO>());

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.Empty(result.Value);
        }
    }
}
