using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetById;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetByIdHistoricalContextTest
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        public GetByIdHistoricalContextTest()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private const int _id = 1;

        private readonly HistoricalContext context = new HistoricalContext
        {
            Id = _id,
            Title = "some title 1"
        };

        private readonly HistoricalContextDTO contextDto = new HistoricalContextDTO
        {
            Id = _id,
            Title = "some title 1"
        };

        async Task SetupRepository(HistoricalContext context)
        {
            _mockRepo.Setup(repo => repo.HistoricalContextRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                    It.IsAny<Func<IQueryable<HistoricalContext>,
                        IIncludableQueryable<HistoricalContext, object>>>()))
                .ReturnsAsync(context);
        }

        async Task SetupMapper(HistoricalContextDTO contextDto)
        {
            _mockMapper.Setup(x => x.Map<HistoricalContextDTO>(It.IsAny<HistoricalContext>()))
                .Returns(contextDto);
        }

        [Fact]
        public async Task Handler_Returns_Matching_Element()
        {
            //Arrange
            await SetupRepository(context);
            await SetupMapper(contextDto);

            var handler = new GetHistoricalContextByIdHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.True(result.Value.Id.Equals(_id)));
        }

        [Fact]
        public async Task Handler_Returns_NoMatching_Element()
        {
            //Arrange
            await SetupRepository(new HistoricalContext());
            await SetupMapper(new HistoricalContextDTO());

            var handler = new GetHistoricalContextByIdHandler(_mockMapper.Object, _mockRepo.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new GetHistoricalContextByIdQuery(_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<HistoricalContextDTO>(result.Value),
                () => Assert.False(result.Value.Id.Equals(_id)));
        }
    }
}