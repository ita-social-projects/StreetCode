using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class DeleteStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> mockLocalizerCannotFind;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToDelete;

        public DeleteStreetcodeHandlerTests()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this._mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            // Arrange
            var testStreetcode = new StreetcodeContent();
            var relatedFigure = new RelatedFigure();
            int testSaveChangesSuccess = 1;

            this.RepositorySetup(testStreetcode, relatedFigure, testSaveChangesSuccess);

            var handler = new DeleteStreetcodeHandler(this._repository.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsNullError(int id)
        {
            // Arrange
            string expectedErrorMessage = $"Cannot find a streetcode with corresponding categoryId: {id}";
            this.mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
               .Returns((string key, object[] args) =>
               {
                   if (args != null && args.Length > 0 && args[0] is int id)
                   {
                       return new LocalizedString(key, $"Cannot find a streetcode with corresponding categoryId: {id}");
                   }

                   return new LocalizedString(key, "Cannot find any streetcode with unknown categoryId");
               });

            int testSaveChangesSuccess = 1;
            var relatedFigure = new RelatedFigure();

            this.RepositorySetup(null!, relatedFigure, testSaveChangesSuccess);
            var handler = new DeleteStreetcodeHandler(this._repository.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSaveAsyncError(int id)
        {
            // Arrange
            var testStreetcode = new StreetcodeContent();
            string expectedErrorMessage = "Failed to delete a streetcode";
            this._mockLocalizerFailedToDelete.Setup(x => x["FailedToDeleteStreetcode"])
          .Returns(new LocalizedString("FailedToDeleteStreetcode", expectedErrorMessage));

            int testSaveChangesFailed = -1;
            var relatedFigure = new RelatedFigure();

            this.RepositorySetup(testStreetcode, relatedFigure, testSaveChangesFailed);

            var handler = new DeleteStreetcodeHandler(this._repository.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this.mockLocalizerCannotFind.Object);

            // Act
            var result = await handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedErrorMessage, result.Errors.Single().Message);
        }

        private void RepositorySetup(StreetcodeContent streetcodeContent, RelatedFigure relatedFigure, int saveChangesVariable)
        {
            this._repository.Setup(x => x.StreetcodeRepository.Delete(streetcodeContent));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
            this._repository.Setup(x => x.StreetcodeRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>())).ReturnsAsync(streetcodeContent);

            this._repository.Setup(x => x.RelatedFigureRepository.Delete(relatedFigure));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
            this._repository.Setup(x => x.RelatedFigureRepository.GetFirstOrDefaultAsync(
                It.IsAny<Expression<Func<RelatedFigure, bool>>>(),
                It.IsAny<Func<IQueryable<RelatedFigure>, IIncludableQueryable<RelatedFigure, object>>>())).ReturnsAsync(relatedFigure);
        }
    }
}
