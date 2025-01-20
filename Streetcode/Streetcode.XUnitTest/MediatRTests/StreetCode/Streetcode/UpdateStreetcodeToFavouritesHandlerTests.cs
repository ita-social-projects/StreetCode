using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateToFavourites;
using Streetcode.DAL.Entities.Users.Favourites;
using Microsoft.IdentityModel.Tokens;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class UpdateStreetcodeToFavouritesHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _logger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _stringLocalizerCannotFind;

        public UpdateStreetcodeToFavouritesHandlerTests()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._logger = new Mock<ILoggerService>();
            this._stringLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsErrorCannotFind()
        {
            // Arrange
            this.SetupStreetcodeRepository(new List<StreetcodeContent>());
            string expectedError = "Cannot find any streetcode with corresponding id";
            this._stringLocalizerCannotFind.Setup(x => x["CannotFindAnyStreetcodeWithCorrespondingId", It.IsAny<object[]>()])
                .Returns(new LocalizedString("CannotFindAnyStreetcodeWithCorrespondingId", expectedError));
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            var handler = new CreateFavouriteStreetcodeHandler(this._repository.Object, this._logger.Object, this._stringLocalizerCannotFind.Object);

            // Act 
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsErrorAlreadyInFavourites()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this.SetupStreetcodeRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = streetcodeId,
                }
            });

            this.SetupFavouritesRepository(new List<Favourites>
            {
                new Favourites
                {
                    StreetcodeId = streetcodeId,
                    UserId = userId,
                }
            });

            string expectedError = "Streetcode is already in favourites";

            var handler = new CreateFavouriteStreetcodeHandler(
                this._repository.Object,
                this._logger.Object,
                this._stringLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this.SetupStreetcodeRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = streetcodeId,
                }
            });

            this.SetupFavouritesRepository(new List<Favourites>());
            this._repository.Setup(x => x.FavouritesRepository.CreateAsync(new Favourites()));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new CreateFavouriteStreetcodeHandler(
                this._repository.Object,
                this._logger.Object,
                this._stringLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task Handle_ReturnsCannotSave()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this.SetupStreetcodeRepository(new List<StreetcodeContent>
            {
                new StreetcodeContent
                {
                    Id = streetcodeId,
                }
            });

            this.SetupFavouritesRepository(new List<Favourites>());
            this._repository.Setup(x => x.FavouritesRepository.CreateAsync(new Favourites()));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            string expectedError = "Cannot save updated streetcode";

            var handler = new CreateFavouriteStreetcodeHandler(
                this._repository.Object,
                this._logger.Object,
                this._stringLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }
        private void SetupStreetcodeRepository(List<StreetcodeContent> returnList)
        {
            this._repository.Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList);
            if (!returnList.IsNullOrEmpty()){
                this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList[0]);
            }
        }

        private void SetupFavouritesRepository(List<Favourites> returnList)
        {
            this._repository.Setup(repo => repo.FavouritesRepository.GetAllAsync(
                It.IsAny<Expression<Func<Favourites, bool>>>(),
                It.IsAny<Func<IQueryable<Favourites>,
                IIncludableQueryable<Favourites, object>>>()))
                .ReturnsAsync(returnList);
            if (!returnList.IsNullOrEmpty())
            {
                this._repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourites, bool>>?>(), It.IsAny<Func<IQueryable<Favourites>, IIncludableQueryable<Favourites, object>>>()))
                .ReturnsAsync(returnList[0]);
            }
        }
    }
}
