using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.TimelineItem.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.TimelineItemTests
{
    public class GetTimelineItemByIdTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockStringLocalizer;

        public GetTimelineItemByIdTest()
        {
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockStringLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsTimelineItem(int id)
        {
            // Arrange
            SetupRepository(GetTimelineItem());
            SetupMapper(GetTimelineDto());

            var handler = new GetTimelineItemByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTimelineItemByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(id, result.Value.Id);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsCorrectType(int id)
        {
            // Arrange
            SetupRepository(GetTimelineItem());
            SetupMapper(GetTimelineDto());

            var handler = new GetTimelineItemByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTimelineItemByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.IsType<Result<TimelineItemDTO>>(result);
        }

        [Theory]
        [InlineData(-1)]
        public async Task Handle_WithNonExistentId_ReturnsError(int id)
        {
            // Arrange
            SetupRepository(null);

            string expectedError = $"Cannot find a timeline item with corresponding id: {id}";
            SetupLocalizer(expectedError);

            var handler = new GetTimelineItemByIdHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockStringLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetTimelineItemByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private static TimelineItem GetTimelineItem()
        {
            return new TimelineItem
            {
                Id = 1,
            };
        }

        private static TimelineItemDTO GetTimelineDto()
        {
            return new TimelineItemDTO
            {
                Id = 1,
            };
        }

        private void SetupRepository(TimelineItem? returnItem)
        {
            _mockRepository
                 .Setup(repo => repo.TimelineRepository.GetFirstOrDefaultAsync(
                     It.IsAny<Expression<Func<TimelineItem, bool>>>(),
                     It.IsAny<Func<IQueryable<TimelineItem>,
                     IIncludableQueryable<TimelineItem, object>>>()))
                 .ReturnsAsync(returnItem);
        }

        private void SetupMapper(TimelineItemDTO returnItem)
        {
            _mockMapper
                 .Setup(x => x.Map<TimelineItemDTO>(
                     It.IsAny<object>()))
                 .Returns(returnItem);
        }

        private void SetupLocalizer(string expectedError)
        {
            _mockStringLocalizer
                .Setup(x => x[
                    It.IsAny<string>(), It.IsAny<object>()])
                .Returns((string key, object[] args) =>
                {
                    if (args.Length > 0)
                    {
                        return new LocalizedString(key, expectedError);
                    }

                    return new LocalizedString(key, "Cannot find a timeline item with unknown id");
                });
        }
    }
}
