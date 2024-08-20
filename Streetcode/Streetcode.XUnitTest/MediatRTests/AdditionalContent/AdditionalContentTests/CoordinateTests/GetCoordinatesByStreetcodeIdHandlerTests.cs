using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.CoordinateTests
{
    public class GetCoordinatesByStreetcodeIdHandlerTests
    {
        private const int _streetcode_id = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly List<StreetcodeCoordinate> coordinates = new List<StreetcodeCoordinate>
        {
            new StreetcodeCoordinate
            {
                Id = 1,
                StreetcodeId = _streetcode_id,
            },
            new StreetcodeCoordinate
            {
                Id = 2,
                StreetcodeId = _streetcode_id,
            },
        };

        private readonly List<StreetcodeCoordinateDTO> coordinateDTOs = new List<StreetcodeCoordinateDTO>
        {
            new StreetcodeCoordinateDTO
            {
                Id = 1,
                StreetcodeId = _streetcode_id,
            },
            new StreetcodeCoordinateDTO
            {
                Id = 2,
                StreetcodeId = _streetcode_id,
            },
        };

        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            this._mockRepo = new Mock<IRepositoryWrapper>();
            this._mockMapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.coordinates, new StreetcodeContent());
            this.SetupMapper(this.coordinateDTOs);

            var handler = new GetCoordinatesByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDTO>>(result.Value),
                () => Assert.True(result.Value.All(co => co.StreetcodeId.Equals(_streetcode_id))));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeCoordinate>(), new StreetcodeContent());
            this.SetupMapper(new List<StreetcodeCoordinateDTO>());

            var expectedError = $"Cannot find coordinates by streetcodeId: {_streetcode_id}";
            this._mockLocalizer.Setup(localizer => localizer["CannotFindCoordinatesByStreetcodeId", _streetcode_id])
                .Returns(new LocalizedString("CannotFindCoordinatesByStreetcodeId", expectedError));

            var handler = new GetCoordinatesByStreetcodeIdHandler(this._mockRepo.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeCoordinate> returnList, StreetcodeContent streetcode)
        {
            this._mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(returnList);

            this._mockRepo.Setup(x => x.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
        }

        private void SetupMapper(List<StreetcodeCoordinateDTO> returnList)
        {
            this._mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
