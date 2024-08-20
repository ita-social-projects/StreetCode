using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
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
            this._mockMapper = new Mock<IMapper>();
            this._mockRepository = new Mock<IRepositoryWrapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            this.SetupMapMethod(GetListPositionDTO());
            this.SetupGetAllAsyncMethod(GetPositionsList());

            var handler = new GetAllPositionsHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<PositionDTO>>(result.ValueOrDefault));
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            // Arrange
            this.SetupMapMethod(GetListPositionDTO());
            this.SetupGetAllAsyncMethod(GetPositionsList());

            var handler = new GetAllPositionsHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetPositionsList().Count(), result.Value.Count()));
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenIdNotExist()
        {
            // Arrange
            const string expectedError = "Cannot find any positions";
            this._mockLocalizerCannotFind.Setup(x => x["CannotFindAnyPositions"])
               .Returns(new LocalizedString("CannotFindAnyPositions", expectedError));

            this.SetupGetAllAsyncMethod(GetPositionsListWithNotExistingId());

            var handler = new GetAllPositionsHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this._mockMapper.Verify(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()), Times.Never);
        }

        private static IEnumerable<Positions> GetPositionsList()
        {
            var partners = new List<Positions>
            {
                new Positions
                {
                    Id = 1,
                },
                new Positions
                {
                    Id = 2,
                },
            };
            return partners;
        }

        private static List<Positions> GetPositionsListWithNotExistingId()
        {
            return new List<Positions>();
        }

        private static List<PositionDTO> GetListPositionDTO()
        {
            var positionDTO = new List<PositionDTO>
            {
                new PositionDTO
                {
                    Id = 1,
                },
                new PositionDTO
                {
                    Id = 2,
                },
            };
            return positionDTO;
        }

        private void SetupMapMethod(IEnumerable<PositionDTO> positionDTOs)
        {
            this._mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
                .Returns(positionDTOs);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
        {
            this._mockRepository.Setup(x => x.PositionRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Positions>, IIncludableQueryable<Positions, object>>>()))
                .ReturnsAsync(positions);
        }
    }
}
