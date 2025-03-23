using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.TimelineItem.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.TimelineItemTests
{
    public class GetAllTimelineItemsTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizer;

        public GetAllTimelineItemsTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockStringLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_CorrectType()
        {
            // Arrange
            SetupRepository(GetTimelinesList());
            SetupMapper(GetTimelineDtoList());

            var handler = new GetAllTimelineItemsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTimelineItemsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.IsAssignableFrom<IEnumerable<TimelineItemDTO>>(result.Value));
        }

        [Fact]
        public async Task ReturnsSuccessfully_AllTimelines()
        {
            // Arrange
            SetupRepository(GetTimelinesList());
            SetupMapper(GetTimelineDtoList());

            var handler = new GetAllTimelineItemsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTimelineItemsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.Equal(GetTimelinesList().Count(), result.Value.Count()));
        }

        [Fact]
        public async Task ReturnsError_IsNull()
        {
            // Arrange
            SetupRepository(null);

            var expectedError = $"Cannot find any timeline item";
            SetupLocalizer(expectedError);

            var handler = new GetAllTimelineItemsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetAllTimelineItemsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.False(result.IsSuccess),
            () => Assert.Equal(expectedError, result.Errors.Single().Message));
        }

        private static IEnumerable<TimelineItem> GetTimelinesList()
        {
            var timelines = new List<TimelineItem>
            {
                new () { Id = 1, },
                new () { Id = 2, },
                new () { Id = 3, },
            };

            return timelines;
        }

        private static IEnumerable<TimelineItemDTO> GetTimelineDtoList()
        {
            var timelines = new List<TimelineItemDTO>
            {
                new () { Id = 1, },
                new () { Id = 2, },
                new () { Id = 3, },
            };

            return timelines;
        }

        private void SetupRepository(IEnumerable<TimelineItem>? returnList)
        {
            _mockRepository
                .Setup(repo => repo.TimelineRepository.GetAllAsync(
                    It.IsAny<Expression<Func<TimelineItem, bool>>>(),
                    It.IsAny<Func<IQueryable<TimelineItem>, IIncludableQueryable<TimelineItem, object>>>()))
                .ReturnsAsync(returnList ?? default!);
        }

        private void SetupMapper(IEnumerable<TimelineItemDTO> returnList)
        {
            _mockMapper
                .Setup(x => x.Map<IEnumerable<TimelineItemDTO>>(
                    It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        private void SetupLocalizer(string expectedError)
        {
            _mockStringLocalizer.Setup(localizer => localizer["CannotFindAnyTimelineItem"])
                .Returns(new LocalizedString("CannotFindAnyTimelineItem", expectedError));
        }
    }
}
