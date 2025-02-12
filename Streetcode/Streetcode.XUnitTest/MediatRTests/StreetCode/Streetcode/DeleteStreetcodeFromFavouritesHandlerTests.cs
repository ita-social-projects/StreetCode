using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System.Linq.Expressions;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class DeleteStreetcodeFromFavouritesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _loggerService;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _stringLocalizerCannotFind;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _stringLocalizerFailedToDelete;

        public DeleteStreetcodeFromFavouritesHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _loggerService = new Mock<ILoggerService>();
            _stringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            _stringLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsErrorIsNotInFavourites()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(new StreetcodeContent());

            this._repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourite, bool>>?>(), It.IsAny<Func<IQueryable<Favourite>, IIncludableQueryable<Favourite, object>>>()))
                .ReturnsAsync((Favourite)null);

            string errorMsg = "Cannot find streetcode in favourites";
            this._stringLocalizerCannotFind.Setup(x => x["CannotFindStreetcodeInFavourites"])
                .Returns(new LocalizedString("CannotFindStreetcodeInFavourites", errorMsg));

            var handler = new DeleteStreetcodeFromFavouritesHandler(
                this._repository.Object,
                this._loggerService.Object,
                this._stringLocalizerCannotFind.Object,
                this._stringLocalizerFailedToDelete.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(errorMsg, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsErrorFailedToDelete()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
               .ReturnsAsync(new StreetcodeContent());

            this._repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourite, bool>>?>(), It.IsAny<Func<IQueryable<Favourite>, IIncludableQueryable<Favourite, object>>>()))
                .ReturnsAsync(new Favourite());

            this._repository.Setup(x => x.FavouritesRepository.Delete(new Favourite()));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);

            string expectedErrorMessage = "Failed to delete a streetcode";
            this._stringLocalizerFailedToDelete.Setup(x => x["FailedToDeleteStreetcode"])
          .Returns(new LocalizedString("FailedToDeleteStreetcode", expectedErrorMessage));

            var handler = new DeleteStreetcodeFromFavouritesHandler(
                this._repository.Object,
                this._loggerService.Object,
                this._stringLocalizerCannotFind.Object,
                this._stringLocalizerFailedToDelete.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
               It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
               .ReturnsAsync(new StreetcodeContent());

            this._repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourite, bool>>?>(), It.IsAny<Func<IQueryable<Favourite>, IIncludableQueryable<Favourite, object>>>()))
                .ReturnsAsync(new Favourite());

            this._repository.Setup(x => x.FavouritesRepository.Delete(new Favourite()));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new DeleteStreetcodeFromFavouritesHandler(
               this._repository.Object,
               this._loggerService.Object,
               this._stringLocalizerCannotFind.Object,
               this._stringLocalizerFailedToDelete.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}
