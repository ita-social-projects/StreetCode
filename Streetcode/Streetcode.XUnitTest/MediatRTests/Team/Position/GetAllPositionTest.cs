using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Helpers;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position
{
    public class GetAllPositionTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;

        public GetAllPositionTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            SetupMapper(GetListPositionDto());
            SetupPaginatedRepository(GetPositionsList());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<PositionDTO>>(result.ValueOrDefault.Positions));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            // Arrange
            SetupMapper(GetListPositionDto());
            SetupPaginatedRepository(GetPositionsList());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetPositionsList().Count(), result.Value.Positions.Count()));
        }

        [Fact]
        public async Task Handler_Returns_Correct_PageSize()
        {
            // Arrange
            ushort pageSize = 3;
            SetupPaginatedRepository(GetPositionsList().Take(pageSize));
            SetupMapper(GetListPositionDto().Take(pageSize).ToList());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(page: 1, pageSize: pageSize), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<PositionDTO>>(result.Value.Positions),
                () => Assert.Equal(pageSize, result.Value.Positions.Count()));
        }

        private static IEnumerable<Positions> GetPositionsList()
        {
            var positions = new List<Positions>
            {
                new Positions
                {
                    Id = 1,
                },
                new Positions
                {
                    Id = 2,
                },
                new Positions
                {
                    Id = 3,
                },
                new Positions
                {
                    Id = 4,
                },
                new Positions
                {
                    Id = 5,
                },
            };

            return positions;
        }

        private static List<PositionDTO> GetListPositionDto()
        {
            var positionDtOs = new List<PositionDTO>
            {
                new PositionDTO
                {
                    Id = 1,
                },
                new PositionDTO
                {
                    Id = 2,
                },
                new PositionDTO
                {
                    Id = 3,
                },
                new PositionDTO
                {
                    Id = 4,
                },
                new PositionDTO
                {
                    Id = 5,
                },
            };

            return positionDtOs;
        }

        private void SetupMapper(IEnumerable<PositionDTO> positionDtOs)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
                .Returns(positionDtOs);
        }

        private void SetupPaginatedRepository(IEnumerable<Positions> returnList)
        {
            _mockRepository.Setup(repo => repo.PositionRepository.GetAllPaginated(
                It.IsAny<ushort?>(),
                It.IsAny<ushort?>(),
                It.IsAny<Expression<Func<Positions, Positions>>?>(),
                It.IsAny<Expression<Func<Positions, bool>>?>(),
                It.IsAny<Func<IQueryable<Positions>, IIncludableQueryable<Positions, object>>?>(),
                It.IsAny<Expression<Func<Positions, object>>?>(),
                It.IsAny<Expression<Func<Positions, object>>?>()))
            .Returns(PaginationResponse<Positions>.Create(returnList.AsQueryable()));
        }
    }
}
