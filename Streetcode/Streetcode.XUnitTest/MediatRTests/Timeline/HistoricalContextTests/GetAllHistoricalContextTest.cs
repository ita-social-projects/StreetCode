using Xunit;
using AutoMapper;
using Moq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.MediatR.Timeline.HistoricalContext.GetAll;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.GetAll;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Helpers;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Team.GetAll;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;

namespace Streetcode.XUnitTest.MediatRTests.Timeline.HistoricalContextTests
{
	public class GetAllHistoricalContextTest
	{
		public Mock<IRepositoryWrapper> _mockRepository;
		public Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllHistoricalContextTest()
		{
			_mockRepository = new();
			_mockMapper = new();
            _mockLogger = new Mock<ILoggerService>();
			_mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_CorrectType() 
		{
            //Arrange
            SetupPaginatedRepository(GetListHistoricalContext());
            SetupMapper(GetListHistoricalContextDTO());
            var hendler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

			//Act
			var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.IsType<List<HistoricalContextDTO>>(result.ValueOrDefault.HistoricalContexts)
				);
		}

		[Fact]
		public async Task ShouldReturnSuccessfully_CountMatch() 
		{
			//Arrange
			SetupPaginatedRepository(GetListHistoricalContext());
			SetupMapper(GetListHistoricalContextDTO());

			var hendler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);
			
			//Act
			var result = await hendler.Handle(new GetAllHistoricalContextQuery(), CancellationToken.None);

			//Assert
			Assert.Multiple(
				() => Assert.NotNull(result),
				() => Assert.Equal(GetListHistoricalContext().Count(), result.Value.HistoricalContexts.Count())
				);  
			}

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            //Arrange
            ushort pageSize = 3;
            SetupPaginatedRepository(GetListHistoricalContext().Take(pageSize));
            SetupMapper(GetListHistoricalContextDTO().Take(pageSize).ToList());

            var handler = new GetAllHistoricalContextHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            //Act
            var result = await handler.Handle(new GetAllHistoricalContextQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<HistoricalContextDTO>>(result.Value.HistoricalContexts),
                () => Assert.Equal(pageSize, result.Value.HistoricalContexts.Count()));
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

        private static IEnumerable<HistoricalContextDTO> GetListHistoricalContextDTO()
        {
            var historicalContextsDTO = new List<HistoricalContextDTO>
            {
                new HistoricalContextDTO { Id = 1, Title = "HistoricalContext1" },
                new HistoricalContextDTO { Id = 2, Title = "HistoricalContext2" },
                new HistoricalContextDTO { Id = 3, Title = "HistoricalContext3" },
                new HistoricalContextDTO { Id = 4, Title = "HistoricalContext4" },
                new HistoricalContextDTO { Id = 5, Title = "HistoricalContext5" },
            };

            return historicalContextsDTO;
        }
    }
}
