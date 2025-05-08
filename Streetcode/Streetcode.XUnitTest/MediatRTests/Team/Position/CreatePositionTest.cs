using AutoMapper;
using Moq;
using Streetcode.BLL.DTO.Team;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Team.Create;
using Streetcode.DAL.Entities.Team;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Team.Position;

public class CreatePositionTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepo;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly MockFailedToValidateLocalizer _mockFailedToValidateLocalizer;
    private readonly MockFieldNamesLocalizer _mockFieldNamesLocalizer;
    private readonly CreatePositionHandler _handler;

    public CreatePositionTest()
    {
        _mockFailedToValidateLocalizer = new MockFailedToValidateLocalizer();
        _mockFieldNamesLocalizer = new MockFieldNamesLocalizer();
        _mockRepo = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
        _handler = new CreatePositionHandler(
            _mockMapper.Object,
            _mockRepo.Object,
            _mockLogger.Object,
            _mockFailedToValidateLocalizer,
            _mockFieldNamesLocalizer);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        var testPosition = GetPosition();
        var testPositionDto = GetPositionDto();
        var testPositionCreateDto = GetPositionCreateDto();

        _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionCreateDTO>()))
            .Returns(testPosition);
        _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(testPositionDto);

        _mockRepo.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPosition.Id)))
            .ReturnsAsync(testPosition);
        _mockRepo.Setup(x => x.SaveChanges())
            .Returns(1);

        // Act
        var result = await _handler.Handle(new CreatePositionQuery(testPositionCreateDto), CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.IsType<PositionDTO>(result.Value);
        Assert.Equal(testPositionDto, result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_IsCorrectAndSuccess()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.PositionRepository.CreateAsync(new Positions()));
        _mockRepo.Setup(repo => repo.SaveChangesAsync()).ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(new PositionDTO());

        // Act
        var result = await _handler.Handle(new CreatePositionQuery(new PositionCreateDTO()), CancellationToken.None);

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

        _mockMapper.Setup(x => x.Map<Positions>(It.IsAny<PositionCreateDTO>()))
            .Returns(testPosition);
        _mockMapper.Setup(x => x.Map<PositionDTO>(It.IsAny<Positions>()))
            .Returns(GetPositionDto());

        _mockRepo.Setup(x => x.PositionRepository.CreateAsync(It.Is<Positions>(y => y.Id == testPosition.Id)))
            .ReturnsAsync(testPosition);

        _mockRepo.Setup(x => x.SaveChangesAsync())
            .Throws(new Exception(expectedError));

        // Act
        var result = await _handler.Handle(new CreatePositionQuery(GetPositionCreateDto()), CancellationToken.None);

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

    private static PositionDTO GetPositionDto()
    {
        return new PositionDTO()
        {
            Id = 1,
            Position = "position",
        };
    }

    private static PositionCreateDTO GetPositionCreateDto()
    {
        return new PositionCreateDTO();
    }
}