using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
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
        private readonly Mock<ILoggerService> _mockLogger;

        public CreatePositionTest()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_WhenTypeIsCorrect()
        {
            //Arrange
            var testPositions = GetPositions();

            SetupMapMethod(testPositions);
            SetupCreateAsyncMethod(testPositions);
            SetupSaveChangesMethod();

            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

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

            SetupMapMethod(testPositions);
            SetupCreateAsyncMethod(testPositions);
            SetupSaveChangesMethod();

            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

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

            SetupMapMethod(testPositions);
            SetupCreateAsyncMethod(testPositions);
            SetupSaveChangesMethodWithErrorThrow(expectedError);

            var handler = new CreatePositionHandler(_mockMapper.Object, _mockRepository.Object, _mockLogger.Object);

            //Act
            var result = await handler.Handle(new CreatePositionQuery(GetPositionsDTO()), CancellationToken.None);

            //Assert
            Assert.Equal(expectedError, result.Errors.First().Message);

            _mockRepository.Verify(x => x.StreetcodeRepository.GetAllAsync(It.IsAny<Expression<Func<StreetcodeContent, bool>>>(), null), Times.Never);
        }

        private void SetupMapMethod(Positions positions)
        {
            _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionDTO>()))
                .Returns(positions);
            _mockMapper.Setup(x => x.Map<PositionCreateDTO>(It.IsAny<Positions>()))
                .Returns(GetPositionsDTO());
        }

        private void SetupCreateAsyncMethod(Positions positions)
        {
            _mockRepository.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == positions.Id)))
                .ReturnsAsync(positions);
        }

        private void SetupSaveChangesMethod() 
        {
            _mockRepository.Setup(x => x.SaveChanges())
                .Returns(1);
        }

        private void SetupSaveChangesMethodWithErrorThrow(string expectedError)
        {
            _mockRepository.Setup(x => x.SaveChanges())
                 .Throws(new Exception(expectedError));
        }

        private static Positions GetPositions()
        {
            return new Positions()
            {
                Id = 1
            };
        }

        private static PositionCreateDTO GetPositionsDTO()
        {
            return new PositionCreateDTO();
        }
    }
}
