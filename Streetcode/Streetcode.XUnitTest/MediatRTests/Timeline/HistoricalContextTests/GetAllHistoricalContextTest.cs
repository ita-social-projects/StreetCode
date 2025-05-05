using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetAllHistoricalContextTest
    {
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;

        public GetAllHistoricalContextTest()
        {
            _mockRepository = new ();
            _mockMapper = new ();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            this.SetupPaginatedRepository(GetListHistoricalContext());
            this.SetupMapper(GetListHistoricalContextDTO());
            var handler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<HistoricalContextDTO>>(result.ValueOrDefault.HistoricalContexts));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            this.SetupPaginatedRepository(GetListHistoricalContext());
            this.SetupMapper(GetListHistoricalContextDTO());

            var handler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetListHistoricalContext().Count(), result.Value.HistoricalContexts.Count()));
            }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            // Arrange
            ushort pageSize = 3;
            this.SetupPaginatedRepository(GetListHistoricalContext().Take(pageSize));
            this.SetupMapper(GetListHistoricalContextDTO().Take(pageSize).ToList());

            var handler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(UserRole.User, page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<HistoricalContextDTO>>(result.Value.HistoricalContexts),
                () => Assert.Equal(pageSize, result.Value.HistoricalContexts.Count()));
        }

        private static IQueryable<HistoricalContext> GetListHistoricalContext()
        {
            var historicalContexts = new List<HistoricalContext>
            {
                new () { Id = 1, Title = "HistoricalContext1" },
                new () { Id = 2, Title = "HistoricalContext2" },
                new () { Id = 3, Title = "HistoricalContext3" },
                new () { Id = 4, Title = "HistoricalContext4" },
                new () { Id = 5, Title = "HistoricalContext5" },
            };

            return historicalContexts.AsQueryable();
        }

        private static IEnumerable<HistoricalContextDTO> GetListHistoricalContextDTO()
        {
            var historicalContextsDTO = new List<HistoricalContextDTO>
            {
                new () { Id = 1, Title = "HistoricalContext1" },
                new () { Id = 2, Title = "HistoricalContext2" },
                new () { Id = 3, Title = "HistoricalContext3" },
                new () { Id = 4, Title = "HistoricalContext4" },
                new () { Id = 5, Title = "HistoricalContext5" },
            };

            return historicalContextsDTO;
        }

        private void SetupPaginatedRepository(IEnumerable<HistoricalContext> returnList)
        {
            _mockRepository.Setup(repo => repo.HistoricalContextRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<HistoricalContext, HistoricalContext>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, bool>>?>(),
                It.IsAny<Func<IQueryable<HistoricalContext>, IIncludableQueryable<HistoricalContext, object>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, object>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, object>>?>()))
            .Returns(PaginationResponse<HistoricalContext>.Create(returnList.AsQueryable()));
        }

        private void SetupMapper(IEnumerable<HistoricalContextDTO> returnList)
        {
            _mockMapper
                .Setup(x => x.Map<IEnumerable<HistoricalContextDTO>>(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(returnList);
        }
    }
}
