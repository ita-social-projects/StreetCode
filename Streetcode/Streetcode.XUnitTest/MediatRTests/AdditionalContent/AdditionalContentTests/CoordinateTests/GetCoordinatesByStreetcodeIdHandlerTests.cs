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
        private const int StreetcodeId = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;

        private readonly List<StreetcodeCoordinate> _coordinates = new List<StreetcodeCoordinate>
        {
            new StreetcodeCoordinate
            {
                Id = 1,
                StreetcodeId = StreetcodeId,
            },
            new StreetcodeCoordinate
            {
                Id = 2,
                StreetcodeId = StreetcodeId,
            },
        };

        private readonly List<StreetcodeCoordinateDTO> _coordinateDtOs = new List<StreetcodeCoordinateDTO>
        {
            new StreetcodeCoordinateDTO
            {
                Id = 1,
                StreetcodeId = StreetcodeId,
            },
            new StreetcodeCoordinateDTO
            {
                Id = 2,
                StreetcodeId = StreetcodeId,
            },
        };

        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            SetupRepository(_coordinates, new StreetcodeContent());
            SetupMapper(_coordinateDtOs);

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDTO>>(result.Value),
                () => Assert.True(result.Value.All(co => co.StreetcodeId.Equals(StreetcodeId))));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            SetupRepository(new List<StreetcodeCoordinate>(), new StreetcodeContent());
            SetupMapper(new List<StreetcodeCoordinateDTO>());

            var expectedError = $"Cannot find coordinates by streetcodeId: {StreetcodeId}";
            _mockLocalizer.Setup(localizer => localizer["CannotFindCoordinatesByStreetcodeId", StreetcodeId])
                .Returns(new LocalizedString("CannotFindCoordinatesByStreetcodeId", expectedError));

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(StreetcodeId), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeCoordinate> returnList, StreetcodeContent streetcode)
        {
            _mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(returnList);

            _mockRepo.Setup(x => x.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
        }

        private void SetupMapper(List<StreetcodeCoordinateDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
