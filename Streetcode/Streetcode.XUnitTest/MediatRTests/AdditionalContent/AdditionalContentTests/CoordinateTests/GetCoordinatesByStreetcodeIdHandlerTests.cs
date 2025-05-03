using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using MockQueryable.Moq;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.CoordinateTests
{
    public class GetCoordinatesByStreetcodeIdHandlerTests
    {
        private const int StreetcodeId = 1;
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockLocalizer;

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

        private readonly List<StreetcodeContent> _streetcodesUserHaveAccessTo = new List<StreetcodeContent>()
        {
            new StreetcodeContent
            {
                Id = StreetcodeId,
            },
        };

        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockLocalizer = new MockCannotFindLocalizer();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            SetupRepository(_coordinates, new StreetcodeContent(),  _streetcodesUserHaveAccessTo);
            SetupMapper(_coordinateDtOs);

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDTO>>(result.Value),
                () => Assert.True(result.Value.All(co => co.StreetcodeId.Equals(StreetcodeId))));
        }

        [Fact]
        public async Task Handler_NoCoordinatesForStreetcode_ReturnsErrorNoCoordinates()
        {
            // Arrange
            SetupRepository(new List<StreetcodeCoordinate>(), new StreetcodeContent(), _streetcodesUserHaveAccessTo);
            SetupMapper(new List<StreetcodeCoordinateDTO>());

            var expectedError = _mockLocalizer["CannotFindCoordinatesByStreetcodeId", StreetcodeId].Value;

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        [Fact]
        public async Task Handler_NoAccessToStreetcode_ReturnsErrorNoStreetcode()
        {
            // Arrange
            SetupRepository(_coordinates, new StreetcodeContent(), new List<StreetcodeContent>());
            SetupMapper(new List<StreetcodeCoordinateDTO>());

            var expectedError = _mockLocalizer["CannotFindAnyStreetcodeWithCorrespondingId", StreetcodeId].Value;

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLogger.Object, _mockLocalizer);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(StreetcodeId, UserRole.User), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeCoordinate> returnList, StreetcodeContent streetcode, List<StreetcodeContent> streetcodeListUserCanAccess)
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

            _mockRepo.Setup(repo => repo.StreetcodeRepository
                    .FindAll(
                        It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                        It.IsAny<Func<IQueryable<StreetcodeContent>,
                            IIncludableQueryable<StreetcodeContent, object>>>()))
                .Returns(streetcodeListUserCanAccess.AsQueryable().BuildMockDbSet().Object);
        }

        private void SetupMapper(List<StreetcodeCoordinateDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}