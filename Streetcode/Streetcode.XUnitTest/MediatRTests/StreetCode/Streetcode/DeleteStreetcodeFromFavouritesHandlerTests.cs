using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteFromFavourites;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateToFavourites;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Users.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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
        public async Task Handle_ReturnsErrorCannotFind()
        {
            // Arrange
            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync((StreetcodeContent)null);

            string expectedError = "Cannot find any streetcode with corresponding id";
            this._stringLocalizerCannotFind.Setup(x => x["CannotFindAnyStreetcodeWithCorrespondingId", It.IsAny<object[]>()])
                .Returns(new LocalizedString("CannotFindAnyStreetcodeWithCorrespondingId", expectedError));
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            var handler = new DeleteStreetcodeFromFavouritesHandler(this._repository.Object, this._loggerService.Object, this._stringLocalizerCannotFind.Object, this._stringLocalizerFailedToDelete.Object);

            // Act 
            var result = await handler.Handle(new DeleteStreetcodeFromFavouritesCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
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
                It.IsAny<Expression<Func<Favourites, bool>>?>(), It.IsAny<Func<IQueryable<Favourites>, IIncludableQueryable<Favourites, object>>>()))
                .ReturnsAsync((Favourites)null);

            string errorMsg = "Streetcode is not in favourites";

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
                It.IsAny<Expression<Func<Favourites, bool>>?>(), It.IsAny<Func<IQueryable<Favourites>, IIncludableQueryable<Favourites, object>>>()))
                .ReturnsAsync(new Favourites());

            this._repository.Setup(x => x.FavouritesRepository.Delete(new Favourites()));
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
                It.IsAny<Expression<Func<Favourites, bool>>?>(), It.IsAny<Func<IQueryable<Favourites>, IIncludableQueryable<Favourites, object>>>()))
                .ReturnsAsync(new Favourites());

            this._repository.Setup(x => x.FavouritesRepository.Delete(new Favourites()));
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
