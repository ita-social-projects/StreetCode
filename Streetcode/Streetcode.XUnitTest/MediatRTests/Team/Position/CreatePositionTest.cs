using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position
{
    public class CreatePositionTest
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IRepositoryWrapper> _mockRepository;

        public CreatePositionTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            //Arrange
            var testPositions = GetPositions();

            _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionDTO>()))
                .Returns(testPositions);
            _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(GetPositionsDTO());

            _mockRepository.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPositions.Id)))
                .ReturnsAsync(testPositions);
            _mockRepository.Setup(x => x.SaveChanges())
                .Returns(1);
            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreatePositionQuery(GetPositionsDTO()), CancellationToken.None);

            //Assert
            Assert.IsType<PositionDTO>(result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenPartnerAdded()
        {
            //Arrange
            var testPositions = GetPositions();

            _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionDTO>()))
                .Returns(testPositions);
            _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(GetPositionsDTO());

            _mockRepository.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPositions.Id)))
                .ReturnsAsync(testPositions);
            _mockRepository.Setup(x => x.SaveChanges())
                .Returns(1);

            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreatePositionQuery(GetPositionsDTO()), CancellationToken.None);

            //Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldThrowExeption_WhenSaveChangesIsNotSuccessful()
        {
            //Arrange
            var testPositions = GetPositions();
            const string expectedError = "Failed to create a Position";

            _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionDTO>()))
                .Returns(testPositions);
            _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(GetPositionsDTO());

            _mockRepository.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPositions.Id)))
                .ReturnsAsync(testPositions);

            _mockRepository.Setup(x => x.SaveChanges())
                .Throws(new Exception(expectedError));

            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object);

            //Act
            var result = await handler.Handle(new CreatePositionQuery(GetPositionsDTO()), CancellationToken.None);

            //Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private static Positions GetPositions()
        {
            return new Positions()
            {
                Id = 1
            };
        }

        private static PositionDTO GetPositionsDTO()
        {
            return new PositionDTO();
        }
    }
}
