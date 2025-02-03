using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.SubtitleTests
{
    public class GetAllSubtitlesRequestHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly List<Subtitle> subtitles = new List<Subtitle>
        {
            new Subtitle
            {
                Id = 1,
                StreetcodeId = 1,
            },
            new Subtitle
            {
                Id = 2,
                StreetcodeId = 1,
            },
        };

        private readonly List<SubtitleDto> subtitleDTOs = new List<SubtitleDto>
        {
            new SubtitleDto
            {
                Id = 1,
                StreetcodeId = 1,
            },
            new SubtitleDto
            {
                Id = 2,
                StreetcodeId = 1,
            },
        };

        public GetAllSubtitlesRequestHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.subtitles);
            this.SetupMapper(this.subtitleDTOs);

            var handler = new GetAllSubtitlesHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<SubtitleDto>>(result.Value),
                () => Assert.True(result.Value.Count().Equals(this.subtitles.Count)));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupRepository(null);
            this.SetupMapper(new List<SubtitleDto>());

            var expectedError = $"Cannot find any subtitles";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindAnySubtitles"])
                .Returns(new LocalizedString("CannotFindAnySubtitles", expectedError));

            var handler = new GetAllSubtitlesHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllSubtitlesQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<Subtitle> returnList)
        {
            this.mockRepo.Setup(repo => repo.SubtitleRepository.GetAllAsync(
                It.IsAny<Expression<Func<Subtitle, bool>>>(),
                It.IsAny<Func<IQueryable<Subtitle>,
                IIncludableQueryable<Subtitle, object>>>()))
                .ReturnsAsync(returnList);
        }

        private void SetupMapper(List<SubtitleDto> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<SubtitleDto>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
