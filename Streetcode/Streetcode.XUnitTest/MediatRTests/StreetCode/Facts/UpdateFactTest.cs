using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Fact.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Facts;

public class UpdateFactTest
{
    private readonly Mock<IRepositoryWrapper> _mockRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILoggerService> _mockLogger;
    private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
    private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerConvertNull;

    public UpdateFactTest()
    {
        this._mockRepository = new Mock<IRepositoryWrapper>();
        this._mockMapper = new Mock<IMapper>();
        this._mockLogger = new Mock<ILoggerService>();
        this._mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        this._mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenUpdated()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Update(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new UpdateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_TryMapNullRequest()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Update(GetFactWithNotExistId() !));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper
            .Setup(x => x.Map<Fact?>(It.IsAny<FactDto>()))
            .Returns(GetFactWithNotExistId());

        var expectedError = "Cannot convert null to Fact";
        this._mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToFact"])
           .Returns(new LocalizedString("CannotConvertNullToFact", expectedError));

        var handler = new UpdateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTOWithNotExistId() !), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Update(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var expectedError = "Failed to update a fact";
        this._mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateFact"])
           .Returns(new LocalizedString("FailedToUpdateFact", expectedError));

        var handler = new UpdateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        this._mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this._mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new UpdateFactHandler(this._mockRepository.Object, this._mockMapper.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.IsType<Unit>(result.Value);
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
