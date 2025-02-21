using System.Linq.Expressions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.CreateFavourite;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Favourites;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class CreateFavouriteStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _logger;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly MockAlreadyExistLocalizer _mockAlreadyExists;
        private readonly MockCannotSaveLocalizer _mockLocalizerCannotSave;

        public CreateFavouriteStreetcodeHandlerTests()
        {
            _repository = new Mock<IRepositoryWrapper>();
            _logger = new Mock<ILoggerService>();
            _mockAlreadyExists = new MockAlreadyExistLocalizer();
            _mockLocalizerCannotSave = new MockCannotSaveLocalizer();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
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

            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            string expectedError = _mockAlreadyExists["FavouriteAlreadyExists"].Value;

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists,
                _mockLocalizerCannotSave,
                _httpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId), CancellationToken.None);

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

            this.SetupSaveChanges(1);

            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists,
                _mockLocalizerCannotSave,
                _httpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task Handle_ReturnsCannotSave()
        {
            // Arrange
            string userId = Guid.NewGuid().ToString();
            int streetcodeId = 1;

            MockHelpers.SetupMockHttpContextAccessor(_httpContextAccessor, userId);

            this.SetupStreetcodeRepository(new List<StreetcodeContent>
            {
                new () { Id = streetcodeId, },
            });

            this.SetupFavouritesRepository(new List<Favourite>());

            this.SetupSaveChanges(0);

            string expectedError = _mockLocalizerCannotSave["CannotSaveTheData"].Value;

            var handler = new CreateFavouriteStreetcodeHandler(
                _repository.Object,
                _logger.Object,
                _mockAlreadyExists,
                _mockLocalizerCannotSave,
                _httpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new CreateFavouriteStreetcodeCommand(streetcodeId), CancellationToken.None);

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
            if (!returnList.IsNullOrEmpty())
            {
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

            _repository.Setup(x => x.FavouritesRepository.CreateAsync(new Favourite()));
        }

        private void SetupSaveChanges(int result)
        {
            _repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(result);
        }
    }
}
