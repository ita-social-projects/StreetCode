using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position
{
    public class CreatePositionTest
    {
        private readonly Mock<IRepositoryWrapper> mockRepo;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;

        public CreatePositionTest()
        {
            this.mockRepo = new Mock<IRepositoryWrapper>();
            this.mockMapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect()
        {
            // Arrange
            var testPosition = GetPosition();
            var testPositionDTO = GetPositionDTO();
            var testPositionCreateDTO = GetPositionCreateDTO();

            this.mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionCreateDTO>()))
                .Returns(testPosition);
            this.mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(testPositionDTO);

            this.mockRepo.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPosition.Id)))
                .ReturnsAsync(testPosition);
            this.mockRepo.Setup(x => x.SaveChanges())
                .Returns(1);

            var handler = new CreatePositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreatePositionQuery(testPositionCreateDTO), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.IsType<PositionDTO>(result.Value);
            Assert.Equal(testPositionDTO, result.Value);
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
        {
            // Arrange
            this.mockRepo.Setup(repo => repo.PositionRepository.CreateAsync(new Positions()));
            this.mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

            this.mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(new PositionDTO());

            var handler = new CreatePositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreatePositionQuery(new PositionCreateDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<PositionDTO>(result.Value),
                () => Assert.True(result.IsSuccess));
        }

        [Fact]
        public async Task ShouldThrowException_SaveChangesIsNotSuccessful()
        {
            // Arrange
            var testPosition = GetPosition();
            var expectedError = "Failed to create a Position";

            this.mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionCreateDTO>()))
                .Returns(testPosition);
            this.mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
                .Returns(GetPositionDTO());

            this.mockRepo.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPosition.Id)))
                .ReturnsAsync(testPosition);

            this.mockRepo.Setup(x => x.SaveChangesAsync())
                .Throws(new Exception(expectedError));

            var handler = new CreatePositionHandler(this.mockMapper.Object, this.mockRepo.Object, this.mockLogger.Object);

            // Act
            var result = await handler.Handle(new CreatePositionQuery(GetPositionCreateDTO()), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static Positions GetPosition()
        {
            return new Positions()
            {
                Id = 1,
                Position = "position",
            };
        }

        private static PositionDTO GetPositionDTO()
        {
            return new PositionDTO()
            {
                Id = 1,
                Position = "position",
            };
        }

        private static PositionCreateDTO GetPositionCreateDTO()
        {
            return new PositionCreateDTO();
        }
    }
}
