using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.TimelineItemTests
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1413:UseTrailingCommasInMultiLineInitializers", Justification = "Reviewed.")]
    public class GetTimelineItemByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        public GetTimelineItemByStreetcodeIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_NotEmpty()
        {
            // Arrange
            SetupRepository(GetExistingTimelineList());

            SetupMapper(GetTimelineItemDtoList());

            var handler = new GetTimelineItemsByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            int streetcodeId = 1;

            // Act
            var result = await handler.Handle(new GetTimelineItemsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
            () => Assert.NotNull(result),
            () => Assert.True(result.IsSuccess),
            () => Assert.NotEmpty(result.Value));
        }

        [Fact]
        public async Task ReturnsSuccessfully_Empty()
        {
            // Arrange
            SetupRepository(GetEmptyTimelineList());

            var handler = new GetTimelineItemsByStreetcodeIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);
            int streetcodeId = 1;

            // Act
            var result = await handler.Handle(new GetTimelineItemsByStreetcodeIdQuery(streetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<TimelineItemDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<TimelineItemDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        private static List<TimelineItem> GetExistingTimelineList()
        {
            return new List<TimelineItem>
            {
                new ()
                {
                    Id = 1,
                    Date = DateTime.Now,
                    DateViewPattern = DAL.Enums.DateViewPattern.DateMonthYear,
                    Streetcode = new StreetcodeContent
                    {
                        Id = 1,
                    }
                }
            };
        }

        private static List<TimelineItem> GetEmptyTimelineList()
        {
            return new List<TimelineItem>();
        }

        private static List<TimelineItemDTO> GetTimelineItemDtoList()
        {
            return new List<TimelineItemDTO>
            {
                new ()
                {
                    Id = 1,
                    Date = DateTime.Now,
                    DateViewPattern = DAL.Enums.DateViewPattern.DateMonthYear,
                }
            };
        }

        private void SetupRepository(List<TimelineItem> returnedList)
        {
            _mockRepository.Setup(x => x.TimelineRepository.GetAllAsync(
               It.IsAny<Expression<Func<TimelineItem, bool>>>(),
               It.IsAny<Func<IQueryable<TimelineItem>,
               IIncludableQueryable<TimelineItem, object>>>()))
               .ReturnsAsync(returnedList);
        }

        private void SetupMapper(List<TimelineItemDTO> returnedList)
        {
            _mockMapper
            .Setup(x => x
            .Map<IEnumerable<TimelineItemDTO>>(It.IsAny<IEnumerable<TimelineItem>>()))
            .Returns(returnedList);
        }
    }
}
