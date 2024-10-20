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
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFailedToCreate;
    private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

    public CreateFactTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<StreetcodeFactCreateDTO>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.IsType<Unit>(result.Value);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenFactAdded()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<StreetcodeFactCreateDTO>()))
            .Returns(GetFact());

        var handler = new CreateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_WhenTryToAddNull()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper
            .Setup(x => x.Map<Fact?>(It.IsAny<StreetcodeFactCreateDTO>()))
            .Returns(GetFactWithNotExistId());

        var expectedError = "Cannot convert null to Fact";
        this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToFact"])
           .Returns(new LocalizedString("CannotConvertNullToFact", expectedError));

        var handler = new CreateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTOWithNotExistId() !), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<StreetcodeFactCreateDTO>()))
            .Returns(GetFact());

        var expectedError = "Failed to create a fact";
        this.mockLocalizerFailedToCreate.Setup(x => x["FailedToCreateFact"])
            .Returns(new LocalizedString("FailedToCreateFact", expectedError));

        var handler = new CreateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new CreateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    private static Fact GetFact()
    {
        return new Fact();
    }

    private static StreetcodeFactCreateDTO GetFactDTO()
    {
        return new StreetcodeFactCreateDTO();
    }

    private static Fact? GetFactWithNotExistId()
    {
        return null;
    }

    private static StreetcodeFactCreateDTO? GetFactDTOWithNotExistId()
    {
        return null;
    }
}
