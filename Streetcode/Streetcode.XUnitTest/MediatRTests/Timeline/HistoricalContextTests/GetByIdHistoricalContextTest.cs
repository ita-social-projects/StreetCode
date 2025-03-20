using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetByIdHistoricalContextTest
    {
        private const int id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly HistoricalContext _context = new ()
        {
            Id = id,
            Title = "some title 1",
        };

        private readonly HistoricalContextDTO _contextDto = new ()
        {
            Id = id,
            Title = "some title 1",
        };

        public GetByIdHistoricalContextTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            this.SetupRepository(_context);
            this.SetupMapper(_contextDto);

            var handler = new GetHistoricalContextByIdHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            this.SetupRepository(new HistoricalContext());
            this.SetupMapper(new HistoricalContextDTO());

            var handler = new GetHistoricalContextByIdHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(id, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(id)));
        }

        private void SetupRepository(HistoricalContext context)
        {
            _mockRepo
                .Setup(repo => repo.HistoricalContextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                        It.IsAny<Func<IQueryable<HistoricalContext>,
                        IIncludableQueryable<HistoricalContext, object>>>()))
                .ReturnsAsync(context);
        }

        private void SetupMapper(HistoricalContextDTO contextDto)
        {
            _mockMapper
                .Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
                .Returns(contextDto);
        }
    }
}