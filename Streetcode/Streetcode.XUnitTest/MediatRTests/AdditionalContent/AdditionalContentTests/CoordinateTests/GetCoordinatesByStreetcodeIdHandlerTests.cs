using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.AdditionalContent.CoordinateTests
{
    public class GetCoordinatesByStreetcodeIdHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizer;
        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
            _mockLocalizer = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        private const int _streetcode_id = 1;

        private readonly List<StreetcodeCoordinate> coordinates = new List<StreetcodeCoordinate>
        {
            new StreetcodeCoordinate
            {
                Id = 1,
                StreetcodeId = _streetcode_id
            },
            new StreetcodeCoordinate
            {
                Id = 2,
                StreetcodeId = _streetcode_id
            }
        };
        private readonly List<StreetcodeCoordinateDTO> coordinateDTOs = new List<StreetcodeCoordinateDTO>
        {
            new StreetcodeCoordinateDTO
            {
                Id = 1,
                StreetcodeId = _streetcode_id
            },
            new StreetcodeCoordinateDTO
            {
                Id = 2,
                StreetcodeId = _streetcode_id
            }
        };

        async Task SetupRepository(List<StreetcodeCoordinate> returnList)
        {
            _mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(returnList);
        }

        async Task SetupMapper(List<StreetcodeCoordinateDTO> returnList)
        {
            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(returnList);
        }

        [Fact]
        public async Task Handler_Returns_NotEmpty_List()
        {
            //Arrange
            await SetupRepository(coordinates);
            await SetupMapper(coordinateDTOs);
                
            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object,_mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDTO>>(result.Value),
                () => Assert.True(result.Value.All(co => co.StreetcodeId.Equals(_streetcode_id))));
        }

        [Fact]
        public async Task Handler_Returns_Empty_List()
        {
            //Arrange
            await SetupRepository(new List<StreetcodeCoordinate>());
            await SetupMapper(new List<StreetcodeCoordinateDTO>());

            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object, _mockLocalizer.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert

            Assert.Multiple(
                () => Assert.IsType<List<StreetcodeCoordinateDTO>>(result.Value),
                () => Assert.Empty(result.Value));
        }
    }
}
