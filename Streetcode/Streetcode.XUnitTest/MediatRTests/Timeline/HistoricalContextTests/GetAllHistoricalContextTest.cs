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
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
    public class GetAllHistoricalContextTest
    {
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;
        private Mock<IRepositoryWrapper> mockRepository;
        private Mock<IMapper> mockMapper;

        public GetAllHistoricalContextTest()
        {
            this.mockRepository = new ();
            this.mockMapper = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CorrectType()
        {
            // Arrange
            this.SetupPaginatedRepository(GetListHistoricalContext());
            this.SetupMapper(GetListHistoricalContextDTO());
            var hendler = new GetAllHistoricalContextHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<HistoricalContextDto>>(result.ValueOrDefault.HistoricalContexts));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_CountMatch()
        {
            // Arrange
            this.SetupPaginatedRepository(GetListHistoricalContext());
            this.SetupMapper(GetListHistoricalContextDTO());

            var hendler = new GetAllHistoricalContextHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

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

            var handler = new GetAllHistoricalContextHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<HistoricalContextDto>>(result.Value.HistoricalContexts),
                () => Assert.Equal(pageSize, result.Value.HistoricalContexts.Count()));
        }

        private static IQueryable<HistoricalContext> GetListHistoricalContext()
        {
            var historicalContexts = new List<HistoricalContext>
            {
                new HistoricalContext { Id = 1, Title = "HistoricalContext1" },
                new HistoricalContext { Id = 2, Title = "HistoricalContext2" },
                new HistoricalContext { Id = 3, Title = "HistoricalContext3" },
                new HistoricalContext { Id = 4, Title = "HistoricalContext4" },
                new HistoricalContext { Id = 5, Title = "HistoricalContext5" },
            };

            return historicalContexts.AsQueryable();
        }

        private static IEnumerable<HistoricalContextDto> GetListHistoricalContextDTO()
        {
            var historicalContextsDTO = new List<HistoricalContextDto>
            {
                new HistoricalContextDto { Id = 1, Title = "HistoricalContext1" },
                new HistoricalContextDto { Id = 2, Title = "HistoricalContext2" },
                new HistoricalContextDto { Id = 3, Title = "HistoricalContext3" },
                new HistoricalContextDto { Id = 4, Title = "HistoricalContext4" },
                new HistoricalContextDto { Id = 5, Title = "HistoricalContext5" },
            };

            return historicalContextsDTO;
        }

        private void SetupPaginatedRepository(IEnumerable<HistoricalContext> returnList)
        {
            this.mockRepository.Setup(repo => repo.HistoricalContextRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<HistoricalContext, HistoricalContext>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, bool>>?>(),
                It.IsAny<Func<IQueryable<HistoricalContext>, IIncludableQueryable<HistoricalContext, object>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, object>>?>(),
                It.IsAny<Expression<Func<HistoricalContext, object>>?>()))
            .Returns(PaginationResponse<HistoricalContext>.Create(returnList.AsQueryable()));
        }

        private void SetupMapper(IEnumerable<HistoricalContextDto> returnList)
        {
            this.mockMapper
                .Setup(x => x.Map<IEnumerable<HistoricalContextDto>>(It.IsAny<IEnumerable<HistoricalContext>>()))
                .Returns(returnList);
        }
    }
}
