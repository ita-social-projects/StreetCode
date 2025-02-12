using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.CreateFavourite;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class CreateFavouriteStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _logger;
        private readonly Mock<IStringLocalizer<AlreadyExistSharedResource>> _mockAlreadyExists;
        private readonly Mock<IStringLocalizer<CannotSaveSharedResource>> _mockLocalizerCannotSave;

        public CreateFavouriteStreetcodeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _logger = new Mock<ILoggerService>();
            _mockAlreadyExists = new Mock<IStringLocalizer<AlreadyExistSharedResource>>();
            _mockLocalizerCannotSave = new Mock<IStringLocalizer<CannotSaveSharedResource>>();
        }

        [Fact]
        public async Task Handle_ReturnsErrorAlreadyInFavourites()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            this.SetupStreetcodeRepository(new List<StreetcodeContent>
            {
                new () { Id = streetcodeId, },
            });

            this.SetupFavouritesRepository(new List<Favourite>
            {
                new () 
                {
                    StreetcodeId = streetcodeId,
                    UserId = userId,
                },
            });

            this.SetupLocalizers();

            string expectedError = "Streetcode is already in favourites";

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists.Object,
                _mockLocalizerCannotSave.Object);

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
                new () { Id = streetcodeId, },
            });

            this.SetupFavouritesRepository(new List<Favourite>());
            _repository.Setup(x => x.FavouritesRepository.CreateAsync(new Favourite()));
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists.Object,
                _mockLocalizerCannotSave.Object);

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
                new () { Id = streetcodeId, },
            });

            this.SetupFavouritesRepository(new List<Favourite>());
            this.SetupLocalizers();
            _repository.Setup(x => x.FavouritesRepository.CreateAsync(new Favourite()));
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(0);
            string expectedError = "Cannot save the data";

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists.Object,
                _mockLocalizerCannotSave.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId, userId), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedError, result.Errors[0].Message),
                () => Assert.True(result.IsFailed));
        }

        private void SetupStreetcodeRepository(List<StreetcodeContent> returnList)
        {
            _repository.Setup(repo => repo.StreetcodeRepository.GetAllAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>,
                IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList);
            if (!returnList.IsNullOrEmpty()){
                _repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>?>(), It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(returnList[0]);
            }
        }

        private void SetupFavouritesRepository(List<Favourite> returnList)
        {
            _repository.Setup(repo => repo.FavouritesRepository.GetAllAsync(
                It.IsAny<Expression<Func<Favourite, bool>>>(),
                It.IsAny<Func<IQueryable<Favourite>,
                IIncludableQueryable<Favourite, object>>>()))
                .ReturnsAsync(returnList);
            if (!returnList.IsNullOrEmpty())
            {
                _repository.Setup(x => x.FavouritesRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<Favourite, bool>>?>(), It.IsAny<Func<IQueryable<Favourite>, IIncludableQueryable<Favourite, object>>>()))
                .ReturnsAsync(returnList[0]);
            }
        }

        private void SetupLocalizers()
        {
            _mockAlreadyExists.Setup(x => x["FavouriteAlreadyExists"])
               .Returns(new LocalizedString("FavouriteAlreadyExists", "Streetcode is already in favourites"));

            _mockLocalizerCannotSave.Setup(x => x["CannotSaveTheData"])
               .Returns(new LocalizedString("CannotSaveTheData", "Cannot save the data"));

        }
    }
}
