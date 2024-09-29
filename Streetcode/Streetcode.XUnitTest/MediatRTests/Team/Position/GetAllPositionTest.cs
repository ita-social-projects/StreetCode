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
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;

        public GetAllPositionTest()
        {
            this.mockMapper = new Mock<IMapper>();
            this.mockRepository = new Mock<IRepositoryWrapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            // Arrange
            this.SetupMapMethod(GetListPositionDTO());
            this.SetupGetAllAsyncMethod(GetPositionsList());

            var handler = new GetAllPositionsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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

            var handler = new GetAllPositionsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

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
            this.mockLocalizerCannotFind.Setup(x => x["CannotFindAnyPositions"])
               .Returns(new LocalizedString("CannotFindAnyPositions", expectedError));

            this.SetupGetAllAsyncMethod(GetPositionsListWithNotExistingId());

            var handler = new GetAllPositionsHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);

            this.mockMapper.Verify(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()), Times.Never);
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
            this.mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
                .Returns(positionDTOs);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
        {
            this.mockRepository.Setup(x => x.PositionRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Positions>, IIncludableQueryable<Positions, object>>>()))
                .ReturnsAsync(positions);
        }
    }
}
