using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.TimelineItem.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.TimelineItemTests
{
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1413:UseTrailingCommasInMultiLineInitializers", Justification = "Reviewed.")]
    public class GetTimelineItemByStreetcodeIdTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetTimelineItemByStreetcodeIdTest()
        {
            mockRepository = new Mock<IRepositoryWrapper>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILoggerService>();
            mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ReturnsSuccessfully_NotEmpty()
        {
            // Arrange
            this.mockRepository.Setup(x => x.TimelineRepository.GetAllAsync(
                It.IsAny<Expression<Func<TimelineItem, bool>>>(),
                It.IsAny<Func<IQueryable<TimelineItem>,
                IIncludableQueryable<TimelineItem, object>>>()))
                .ReturnsAsync(GetExistingTimelineList());

            this.mockMapper
            .Setup(x => x
            .Map<IEnumerable<TimelineItemDTO>>(It.IsAny<IEnumerable<TimelineItem>>()))
            .Returns(GetTimelineItemDTOList());

            var handler = new GetTimelineItemsByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);
            int streetcodeId = 1;

            // Act
            var result = await handler.Handle(new GetTimelineItemsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

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
            this.mockRepository.Setup(x => x.TimelineRepository.GetAllAsync(
                It.IsAny<Expression<Func<TimelineItem, bool>>>(),
                It.IsAny<Func<IQueryable<TimelineItem>,
                IIncludableQueryable<TimelineItem, object>>>()))
                .ReturnsAsync(GetEmptyTimelineList());

            var handler = new GetTimelineItemsByStreetcodeIdHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);
            int streetcodeId = 1;

            // Act
            var result = await handler.Handle(new GetTimelineItemsByStreetcodeIdQuery(streetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<IEnumerable<TimelineItemDTO>>>(result),
                () => Assert.IsAssignableFrom<IEnumerable<TimelineItemDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }

        public static List<TimelineItem> GetExistingTimelineList()
        {
            return new List<TimelineItem> {
                new TimelineItem
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

        public static List<TimelineItem> GetEmptyTimelineList()
        {
            return new List<TimelineItem>();
        }

        public static List<TimelineItemDTO> GetTimelineItemDTOList()
        {
            return new List<TimelineItemDTO>
            {
                new TimelineItemDTO
                {
                    Id = 1,
                    Date = DateTime.Now,
                    DateViewPattern = DAL.Enums.DateViewPattern.DateMonthYear,
                }
            };
        }
    }
}
