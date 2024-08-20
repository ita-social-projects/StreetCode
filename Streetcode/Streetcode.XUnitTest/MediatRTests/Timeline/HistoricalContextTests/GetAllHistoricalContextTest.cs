using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetAllHistoricalContextTest
    {
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
        private Mock<IRepositoryWrapper> _mockRepository;
        private Mock<IMapper> _mockMapper;

        public GetAllHistoricalContextTest()
        {
            this._mockRepository = new ();
            this._mockMapper = new ();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorectType()
        {
            // Arrange
            (this._mockMapper, this._mockRepository) = GetMapperAndRepo(this._mockMapper, this._mockRepository);
            var hendler = new GetAllHistoricalContextHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<HistoricalContextDTO>>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            (this._mockMapper, this._mockRepository) = GetMapperAndRepo(this._mockMapper, this._mockRepository);
            var hendler = new GetAllHistoricalContextHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetListHistoricalContext().Count(), result.Value.Count()));
        }

        [Fact]
        public async Task ShouldThrowException_WhenNotFound()
        {
            // Arrange
            this._mockRepository.Setup(x => x.HistoricalContextRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                    It.IsAny<Func<IQueryable<HistoricalContext>,
                    IIncludableQueryable<HistoricalContext, object>>>()))
                .ReturnsAsync(GetEmptyListHistoricalContext());

            this._mockMapper
                .Setup(x => x
                .Map<IEnumerable<HistoricalContextDTO>>(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(GetEmptyListHistoricalContextDTO());

            var expectedError = "Cannot find any historical contexts";
            this._mockLocalizerCannotFind.Setup(x => x["CannotFindAnyHistoricalContexts"])
               .Returns(new LocalizedString("CannotFindAnyHistoricalContexts", expectedError));

            var handler = new GetAllHistoricalContextHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.False(result.IsSuccess),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        private static (Mock<IMapper>, Mock<IRepositoryWrapper>) GetMapperAndRepo(
            Mock<IMapper> mockMapper,
            Mock<IRepositoryWrapper> mockRepo)
        {
            mockRepo.Setup(x => x.HistoricalContextRepository
                .GetAllAsync(
                    It.IsAny<Expression<Func<HistoricalContext, bool>>>(),
                    It.IsAny<Func<IQueryable<HistoricalContext>,
                        IIncludableQueryable<HistoricalContext, object>>>()))
                .ReturnsAsync(GetListHistoricalContext());

            mockMapper
                .Setup(x => x
                .Map<IEnumerable<HistoricalContextDTO>>(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(GetListHistoricalContextDTO());

            return (mockMapper, mockRepo);
        }

        private static IQueryable<HistoricalContext> GetListHistoricalContext()
        {
            var historicalContexts = new List<HistoricalContext>()
            {
                new HistoricalContext { Id = 1, Title = "HistoricalContext1" },
                new HistoricalContext { Id = 2, Title = "HistoricalContext2" },
                new HistoricalContext { Id = 3, Title = "HistoricalContext3" },
            };
            return historicalContexts.AsQueryable();
        }

        private static IEnumerable<HistoricalContextDTO> GetListHistoricalContextDTO()
        {
            var historicalContextsDTO = new List<HistoricalContextDTO>()
            {
                new HistoricalContextDTO { Id = 1, Title = "HistoricalContext1" },
                new HistoricalContextDTO { Id = 2, Title = "HistoricalContext2" },
                new HistoricalContextDTO { Id = 3, Title = "HistoricalContext3" },
            };
            return historicalContextsDTO;
        }

        private static List<HistoricalContext> GetEmptyListHistoricalContext() => new List<HistoricalContext>();

        private static List<HistoricalContextDTO> GetEmptyListHistoricalContextDTO() => new List<HistoricalContextDTO>();
    }
}
