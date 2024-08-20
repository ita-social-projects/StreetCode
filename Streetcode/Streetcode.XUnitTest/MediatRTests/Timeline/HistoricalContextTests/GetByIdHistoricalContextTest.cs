using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetByIdHistoricalContextTest
    {
        private const int _id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;

        private readonly HistoricalContext context = new HistoricalContext
        {
            Id = _id,
            Title = "some title 1",
        };

        private readonly HistoricalContextDTO contextDto = new HistoricalContextDTO
        {
            Id = _id,
            Title = "some title 1",
        };

        public GetByIdHistoricalContextTest()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            // Arrange
            this.SetupRepository(this.context);
            this.SetupMapper(this.contextDto);

            var handler = new GetHistoricalContextByIdHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(_id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            // Arrange
            this.SetupRepository(new HistoricalContext());
            this.SetupMapper(new HistoricalContextDTO());

            var handler = new GetHistoricalContextByIdHandler(this._mockMapper.Object, this._mockRepo.Object, this._mockLogger.Object);

            // Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(_id)));
        }

        private void SetupRepository(HistoricalContext context)
        {
            this._mockRepo
                .Setup(repo => repo.HistoricalContextRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                        It.IsAny<Func<IQueryable<HistoricalContext>,
                        IIncludableQueryable<HistoricalContext, object>>>()))
                .ReturnsAsync(context);
        }

        private void SetupMapper(HistoricalContextDTO contextDto)
        {
            this._mockMapper
                .Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
                .Returns(contextDto);
        }
    }
}