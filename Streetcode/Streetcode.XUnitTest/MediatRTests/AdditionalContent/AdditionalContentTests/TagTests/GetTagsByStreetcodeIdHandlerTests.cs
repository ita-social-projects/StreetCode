using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Types;
using Streetcode.BLL.Interfaces.Logging;
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
        private readonly Mock<ILoggerService> _mockLogger;
        public GetTagsByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        private const int _streetcode_id = 1;

        private readonly List<StreetcodeTagIndex> tags = new List<StreetcodeTagIndex>()
        {
            new StreetcodeTagIndex
            {
                Index = 1,
                IsVisible = true,
                Streetcode =  new StreetcodeContent {
                        Id = _streetcode_id
                    },
                StreetcodeId = _streetcode_id,
                Tag = new Tag()
                {
                    Id  = 1,
                    Title = "title"
                }
            },
            new StreetcodeTagIndex
            {
                Index = 2,
                IsVisible = true,
                Streetcode =  new StreetcodeContent {
                        Id = _streetcode_id
                    },
                StreetcodeId = _streetcode_id,
                Tag = new Tag()
                {
                    Id  = 2,
                    Title = "title"
                }
            }

            
        };
        private readonly List<StreetcodeTagDTO> tagDTOs = new List<StreetcodeTagDTO>()  
        {
            new StreetcodeTagDTO
            {
                Id = 1,
                Title = "title",
                IsVisible = true,
                Index = 1,
            },
            new StreetcodeTagDTO
            {
                Id = 2,
                Title = "title",
                IsVisible = true,
                Index = 2,
            }
        };

        async Task SetupRepository(List<StreetcodeTagIndex> returnList)
        {
            _mockRepo.Setup(repo => repo.StreetcodeTagIndexRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeTagIndex, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeTagIndex>,
                IIncludableQueryable<StreetcodeTagIndex, object>>>()))
                .ReturnsAsync(returnList);
        }
        async Task SetupMapper(List<StreetcodeTagDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeTagDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            await SetupRepository(tags);
            await SetupMapper(tagDTOs);

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            /*Assert.Multiple(
                () => Assert.IsType<List<TagDTO>>(result.Value),
                () => Assert.True(result.Value.All(x => x.Streetcodes.All(y => y.Id == _streetcode_id))));*/
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            await SetupRepository(new List<StreetcodeTagIndex>());
            await SetupMapper(new List<StreetcodeTagDTO>());

            var handler = new GetTagByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetTagByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeTagDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }
    }
}
