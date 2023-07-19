using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            //Arrange
            SetupMapMethod(GetListPositionDTO());
            SetupGetAllAsyncMethod(GetPositionsList());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            //Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.IsType<List<PositionDTO>>(result.ValueOrDefault)
            );
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenCountMatch()
        {
            //Arrange
            SetupMapMethod(GetListPositionDTO());
            SetupGetAllAsyncMethod(GetPositionsList());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            //Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.Equal(GetPositionsList().Count(), result.Value.Count())
            );
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenIdNotExist()
        {
            //Arrange
            const string expectedError = "Cannot find any positions";

            SetupGetAllAsyncMethod(GetPositionsListWithNotExistingId());

            var handler = new GetAllPositionsHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizerCannotFind.Object);

            //Act
            var result = await handler.Handle(new GetAllPositionsQuery(), CancellationToken.None);

            //Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockMapper.Verify(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()), Times.Never);
        }

        private void SetupMapMethod(IEnumerable<PositionDTO> positionDTOs)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<PositionDTO>>(It.IsAny<IEnumerable<Positions>>()))
                .Returns(positionDTOs);
        }

        private void SetupGetAllAsyncMethod(IEnumerable<Positions> positions)
        {
            _mockRepository.Setup(x => x.PositionRepository.GetAllAsync(
                null,
                It.IsAny<Func<IQueryable<Positions>, IIncludableQueryable<Positions, object>>>()))
                .ReturnsAsync(positions);
        }

        private static IEnumerable<Positions> GetPositionsList()
        {
            var partners = new List<Positions>{
                new Positions
                {
                    Id = 1
                },
                new Positions
                {
                    Id = 2
                }
            };
            return partners;
        }

        private static List<Positions>? GetPositionsListWithNotExistingId()
        {
            return null;
        }

        private static List<PositionDTO> GetListPositionDTO()
        {
            var PositionDTO = new List<PositionDTO>{
                new PositionDTO
                {
                    Id = 1
                },
                new PositionDTO
                {
                    Id = 2,
                }
            };
            return PositionDTO;
        }
    }
}
