using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
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
        [Theory]
        [InlineData(13)]
        public async Task GetByStreetcodeId_ReturnsList(int streetcode_id)
        {
            //Arrange
            var coordinates = new List<StreetcodeCoordinate>()
            {
                new StreetcodeCoordinate
                {
                    Id = 1,
                    StreetcodeId = 13
                },
                new StreetcodeCoordinate
                {
                    Id = 2,
                    StreetcodeId = 13
                }
            };

            var mockRepo = new Mock<IRepositoryWrapper>();

           mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
               It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
               It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
               IIncludableQueryable<StreetcodeCoordinate, object>>>()))
               .ReturnsAsync(coordinates);
            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(It.IsAny<IEnumerable<object>>()))
                .Returns((List<StreetcodeCoordinate> coordinates) =>
                {
                    var dtolist = new List<StreetcodeCoordinateDTO>();

                    for (int i = 0; i < coordinates.Count; i++)
                        dtolist.Add(new StreetcodeCoordinateDTO
                        {
                            Id = coordinates[i].Id,
                            StreetcodeId = coordinates[i].StreetcodeId
                        });

                    return dtolist;
                });

            var handler = new GetCoordinatesByStreetcodeIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Value.Any());
            Assert.True(result.Value.All(co => co.StreetcodeId == streetcode_id));
        }

        [Theory]
        [InlineData(11)]
        public async Task GetByStreetcodeId_NoMatchingList(int streetcode_id)
        {
            var mockRepo = new Mock<IRepositoryWrapper>();

            mockRepo.Setup(repo => repo.StreetcodeCoordinateRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeCoordinate, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeCoordinate>,
                IIncludableQueryable<StreetcodeCoordinate, object>>>()))
                .ReturnsAsync(new List<StreetcodeCoordinate>());

            var mockMapper = new Mock<IMapper>();

            mockMapper.Setup(x => x.Map<IEnumerable<StreetcodeCoordinateDTO>>(
                It.IsAny<IEnumerable<object>>()))
                .Returns((List<StreetcodeCoordinate> coordinates) =>
                {
                    var dtolist = new List<StreetcodeCoordinateDTO>();

                    for (int i = 0; i < coordinates.Count; i++)
                        dtolist.Add(new StreetcodeCoordinateDTO
                        {
                            Id = coordinates[i].Id,
                            StreetcodeId = coordinates[i].StreetcodeId
                        });
                    return dtolist;
                });


            var handler = new GetCoordinatesByStreetcodeIdHandler(mockRepo.Object, mockMapper.Object);

            //Act
            var result = await handler.Handle(new GetCoordinatesByStreetcodeIdQuery(streetcode_id), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);

            Assert.False(result.Value.Any());
        }
    }
}
