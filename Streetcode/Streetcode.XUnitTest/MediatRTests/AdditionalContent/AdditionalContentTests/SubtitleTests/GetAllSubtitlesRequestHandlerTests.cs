using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetAllSubtitlesRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllSubtitlesRequestHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        private readonly List<Subtitle> subtitles = new List<Subtitle>
        {
            new Subtitle
            {
                Id = 1,
                Status = SubtitleStatus.Editor,
                StreetcodeId = 1
            },
            new Subtitle
            {
                Id = 2,
                Status = SubtitleStatus.Illustrator,
                StreetcodeId = 1
            }
        };
        private readonly List<SubtitleDTO> subtitleDTOs = new List<SubtitleDTO> 
        {
            new SubtitleDTO
            {
                Id = 1,
                StreetcodeId = 1
            },
            new SubtitleDTO
            {
                Id = 2,
                StreetcodeId = 1
            }
        };

        async Task SetupRepository(List<Subtitle> returnList)
        {
            _mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnList);
        }

        async Task SetupMapper(List<SubtitleDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            await SetupRepository(subtitles);
            await SetupMapper(subtitleDTOs);

            var handler = new GetAllSubtitlesHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);
            
            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<SubtitleDTO>>(result.Value),
                () => Assert.True(result.Value.Count().Equals(subtitles.Count)));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            await SetupRepository(new List<Subtitle>());
            await SetupMapper(new List<SubtitleDTO>());

            var handler = new GetAllSubtitlesHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                    () => Assert.IsType<List<SubtitleDTO>>(result.Value),
                    () => Assert.Empty(result.Value));
        }
    }
}
