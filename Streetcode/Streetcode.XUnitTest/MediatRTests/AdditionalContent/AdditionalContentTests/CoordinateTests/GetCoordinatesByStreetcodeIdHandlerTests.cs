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
        private const int streetcode_id = 1;
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizer;

        private readonly List<StreetcodeCoordinate> coordinates = new List<StreetcodeCoordinate>
        {
            new StreetcodeCoordinate
            {
                Id = 1,
                StreetcodeId = streetcode_id,
            },
            new StreetcodeCoordinate
            {
                Id = 2,
                StreetcodeId = streetcode_id,
            },
        };

        private readonly List<StreetcodeCoordinateDto> coordinateDTOs = new List<StreetcodeCoordinateDto>
        {
            new StreetcodeCoordinateDto
            {
                Id = 1,
                StreetcodeId = streetcode_id,
            },
            new StreetcodeCoordinateDto
            {
                Id = 2,
                StreetcodeId = streetcode_id,
            },
        };

        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            // Arrange
            this.SetupRepository(this.coordinates, new StreetcodeContent());
            this.SetupMapper(this.coordinateDTOs);

            var handler = new GetCoordinatesByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDto>>(result.Value),
                () => Assert.True(result.Value.All(co => co.StreetcodeId.Equals(streetcode_id))));
        }

        [Fact]
        public async Task Handler_Returns_Error()
        {
            // Arrange
            this.SetupRepository(new List<StreetcodeCoordinate>(), new StreetcodeContent());
            this.SetupMapper(new List<StreetcodeCoordinateDto>());

            var expectedError = $"Cannot find coordinates by streetcodeId: {streetcode_id}";
            this.mockLocalizer.Setup(localizer => localizer["CannotFindCoordinatesByStreetcodeId", streetcode_id])
                .Returns(new LocalizedString("CannotFindCoordinatesByStreetcodeId", expectedError));

            var handler = new GetCoordinatesByStreetcodeIdHandler(this.mockRepo.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizer.Object);

            // Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors.Single().Message);
        }

        private void SetupRepository(List<StreetcodeCoordinate> returnList, StreetcodeContent streetcode)
        {
            this.mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(returnList);

            this.mockRepo.Setup(x => x.StreetcodeRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>,
                    IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcode);
        }

        private void SetupMapper(List<StreetcodeCoordinateDto> returnList)
        {
            this.mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDto>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }
    }
}
