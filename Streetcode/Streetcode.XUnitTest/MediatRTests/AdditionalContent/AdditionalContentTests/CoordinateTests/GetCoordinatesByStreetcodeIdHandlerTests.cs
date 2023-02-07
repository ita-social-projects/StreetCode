using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;
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
        public GetCoordinatesByStreetcodeIdHandlerTests()
        {
            _mockRepo = new Mock<IRepositoryWrapper>();
            _mockMapper = new Mock<IMapper>();
        }

        private const int _streetcode_id = 1;

        private readonly List<StreetcodeCoordinate> coordinates = new List<StreetcodeCoordinate>
        {
            new StreetcodeCoordinate
            {
                Id = 1,
                StreetcodeId = 1
            },
            new StreetcodeCoordinate
            {
                Id = 2,
                StreetcodeId = 1
            }
        };
        private readonly List<StreetcodeCoordinateDTO> coordinateDTOs = new List<StreetcodeCoordinateDTO>
        {
            new StreetcodeCoordinateDTO
            {
                Id = 1,
                StreetcodeId = 1
            },
            new StreetcodeCoordinateDTO
            {
                Id = 2,
                StreetcodeId = 1
            }
        };

        [Fact]
        public async Task GetByStreetcodeId_ReturnsList()
        {
            //Arrange
           _mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
               It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
               IIncludableQueryable<StreetcodeCoordinate, object>>>()))
               .ReturnsAsync(coordinates);

            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns(coordinateDTOs);
                
            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Value.All(co => co.StreetcodeId == _streetcode_id));
        }

        [Fact]
        public async Task GetByStreetcodeId_NoMatchingList()
        {
            _mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(new List<StreetcodeCoordinate>());

            _mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(
                It.IsAny<IEnumerable<object>>()))
                .Returns(new List<StreetcodeCoordinateDTO>());


            var handler = new GetCoordinatesByStreetcodeIdHandler(_mockRepo.Object, _mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(_streetcode_id), CancellationToken.None);

            //Assert
            Assert.Empty(result.Value);
        }
    }
}
