using AutoMapper;
using MediatR;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class CreateFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;

    public CreateFactTest()
    {
        _mockRepository = new Mock<IRepositoryWrapper>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILoggerService>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<Fact> (It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        //Assert
        Assert.IsType<Unit>(result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenFactAdded()
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        //Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_WhenTryToAddNull()
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        _mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFactWithNotExistId());

        var expectedError = "Cannot convert null to Fact";

        var handler = new CreateFactHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTOWithNotExistId()), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        //Arrange
        _mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        _mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

        _mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var expectedError = "Failed to create a fact";

        var handler = new CreateFactHandler(_mockRepository.Object, _mockMapper.Object, _mockLogger.Object);

        //Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        //Assert
        Assert.Equal(expectedError, result.Errors.First().Message);
    }

    private static Fact GetFact()
    {
        return new Fact();
    }
    private static FactDto GetFactDTO()
    {
        return new FactDto();
    }
    private static Fact? GetFactWithNotExistId()
    {
        return null;
    }
    private static FactDto? GetFactDTOWithNotExistId()
    {
        return null;
    }
}
