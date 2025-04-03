using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode;

public class DeleteStreetcodeFromFavouritesHandlerTests
{
    private readonly Mock<IRepositoryWrapper> _repository;
    private readonly Mock<ILoggerService> _loggerService;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
    private readonly MockCannotFindLocalizer _stringLocalizerCannotFind;
    private readonly MockFailedToDeleteLocalizer _stringLocalizerFailedToDelete;

    public DeleteStreetcodeFromFavouritesHandlerTests()
    {
        _repository = new Mock<IRepositoryWrapper>();
        _loggerService = new Mock<ILoggerService>();
        _httpContextAccessor = new Mock<IHttpContextAccessor>();
        _stringLocalizerCannotFind = new MockCannotFindLocalizer();
        _stringLocalizerFailedToDelete = new MockFailedToDeleteLocalizer();
    }

    [Fact]
    public async Task Handle_ReturnsErrorIsNotInFavourites()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        const int streetcodeId = 1;

        SetupFavouritesRepository();

        SetupStreetcodeRepository();

        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

        var errorMsg = _stringLocalizerCannotFind["CannotFindStreetcodeInFavourites"].Value;

        var handler = new DeleteStreetcodeFromFavouritesHandler(
            _repository.Object,
            _loggerService.Object,
            _stringLocalizerCannotFind,
            _stringLocalizerFailedToDelete,
            _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(errorMsg, result.Errors[0].Message),
            () => Assert.True(result.IsFailed));
    }

    [Fact]
    public async Task Handle_ReturnsErrorFailedToDelete()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        const int streetcodeId = 1;

        SetupStreetcodeRepository(new StreetcodeContent());

        SetupFavouritesRepository(new Favourite());

        SetupSaveChanges(0);

        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

        string expectedErrorMessage = _stringLocalizerFailedToDelete["FailedToDeleteStreetcode"].Value;

        var handler = new DeleteStreetcodeFromFavouritesHandler(
            _repository.Object,
            _loggerService.Object,
            _stringLocalizerCannotFind,
            _stringLocalizerFailedToDelete,
            _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.Multiple(
            () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message),
            () => Assert.True(result.IsFailed));
    }

    [Fact]
    public async Task Handle_ReturnsSuccess()
    {
        // Arrange
        var userId = Guid.NewGuid().ToString();
        const int streetcodeId = 1;

        SetupStreetcodeRepository(new StreetcodeContent());

        SetupFavouritesRepository(new Favourite());

        SetupSaveChanges(1);

        MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

        var handler = new DeleteStreetcodeFromFavouritesHandler(
            _repository.Object,
            _loggerService.Object,
            _stringLocalizerCannotFind,
            _stringLocalizerFailedToDelete,
            _httpContextAccessor.Object);

        // Act
        var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }

    private void SetupSaveChanges(int result)
    {
        _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(result);
    }

    private void SetupFavouritesRepository(Favourite? returnValue = null)
    {
        _repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourite, bool>>?>(), It.IsAny<Func<IQueryable<Favourite>, IIncludableQueryable<Favourite, object>>>()))
            .ReturnsAsync(returnValue);

        _repository.Setup(x => x.FavouritesRepository.Delete(returnValue ?? new Favourite()));
    }

    private void SetupStreetcodeRepository(StreetcodeContent? returnValue = null)
    {
        _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
            .ReturnsAsync(returnValue);
    }
}