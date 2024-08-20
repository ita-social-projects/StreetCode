using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class CreateFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;
    private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

    public CreateFactTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        this._mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactCreateDTO>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.IsType<Unit>(result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenFactAdded()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactCreateDTO>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_WhenTryToAddNull()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper
            .Setup(x => x.Map<Fact?>(It.IsAny<FactCreateDTO>()))
            .Returns(GetFactWithNotExistId());

        var expectedError = "Cannot convert null to Fact";
        this._mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToFact"])
           .Returns(new LocalizedString("CannotConvertNullToFact", expectedError));

        var handler = new CreateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTOWithNotExistId() !), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactCreateDTO>()))
            .Returns(GetFact());

        var expectedError = "Failed to create a fact";
        this._mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateFact"])
            .Returns(new LocalizedString("FailedToCreateFact", expectedError));

        var handler = new CreateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static Fact GetFact()
    {
        return new Fact();
    }

    private static FactCreateDTO GetFactDTO()
    {
        return new FactCreateDTO();
    }

    private static Fact? GetFactWithNotExistId()
    {
        return null;
    }

    private static FactCreateDTO? GetFactDTOWithNotExistId()
    {
        return null;
    }
}
