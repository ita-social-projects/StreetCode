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
    private readonly Mock<IRepositoryWrapper> mockRepository;
    private readonly Mock<IMapper> mockMapper;
    private readonly Mock<ILoggerService> mockLogger;
    private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
    private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerConvertNull;

    public UpdateFactTest()
    {
        this.mockRepository = new Mock<IRepositoryWrapper>();
        this.mockMapper = new Mock<IMapper>();
        this.mockLogger = new Mock<ILoggerService>();
        this.mockLocalizerConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_WhenUpdated()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Update(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new UpdateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task ShouldThrowExeption_TryMapNullRequest()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Update(GetFactWithNotExistId() !));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper
            .Setup(x => x.Map<Fact?>(It.IsAny<FactDto>()))
            .Returns(GetFactWithNotExistId());

        var expectedError = "Cannot convert null to Fact";
        this.mockLocalizerConvertNull.Setup(x => x["CannotConvertNullToFact"])
           .Returns(new LocalizedString("CannotConvertNullToFact", expectedError));

        var handler = new UpdateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTOWithNotExistId() !), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Update(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(-1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var expectedError = "Failed to update a fact";
        this.mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateFact"])
           .Returns(new LocalizedString("FailedToUpdateFact", expectedError));

        var handler = new UpdateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

        // Act
        var result = await handler.Handle(new UpdateFactCommand(GetFactDTO()), CancellationToken.None);

        // Assert
        Assert.Equal(expectedError, result.Errors[0].Message);
    }

    [Fact]
    public async Task ShouldReturnSuccessfully_TypeIsCorrect()
    {
        // Arrange
        this.mockRepository.Setup(x => x.FactRepository.Create(GetFact()));
        this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

        this.mockMapper.Setup(x => x.Map<Fact>(It.IsAny<FactDto>()))
            .Returns(GetFact());

        var handler = new UpdateFactHandler(this.mockRepository.Object, this.mockMapper.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerConvertNull.Object);

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
