using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.XUnitTest.Mocks;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class DeleteStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryMock;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly MockCannotFindLocalizer _mockCannotFindLocalizer;
        private readonly MockFailedToDeleteLocalizer _mockFailedToDeleteLocalizer;
        private readonly DeleteStreetcodeHandler _handler;

        public DeleteStreetcodeHandlerTests()
        {
            _repositoryMock = new Mock<IRepositoryWrapper>();
            _mockLogger = new Mock<ILoggerService>();
            _mockCannotFindLocalizer = new MockCannotFindLocalizer();
            _mockFailedToDeleteLocalizer = new MockFailedToDeleteLocalizer();

            _handler = new DeleteStreetcodeHandler(
                _repositoryMock.Object,
                _mockLogger.Object,
                _mockFailedToDeleteLocalizer,
                _mockCannotFindLocalizer);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSuccess(int id)
        {
            // Arrange
            var testStreetcode = new StreetcodeContent();
            var relatedFigures = new List<RelatedFigure>();
            int testSaveChangesSuccess = 1;

            SetupRepositoryMocks(testStreetcode, relatedFigures, testSaveChangesSuccess);

            // Act
            var result = await _handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            _repositoryMock.Verify(repo => repo.StreetcodeRepository.Delete(testStreetcode), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsNullError(int id)
        {
            // Arrange
            string expectedErrorKey = "CannotFindAnyStreetcodeWithCorrespondingId";
            string expectedErrorValue = _mockCannotFindLocalizer[expectedErrorKey, id];

            SetupRepositoryMocks(null, new List<RelatedFigure>(), 1);

            // Act
            var result = await _handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains(expectedErrorValue, result.Errors.Single().Message);
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<DeleteStreetcodeCommand>(), It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Theory]
        [InlineData(1)]
        public async Task Handle_ReturnsSaveAsyncError(int id)
        {
            // Arrange
            var testStreetcode = new StreetcodeContent();
            string expectedErrorKey = "FailedToDeleteStreetcode";
            string expectedErrorValue = _mockFailedToDeleteLocalizer[expectedErrorKey];

            SetupRepositoryMocks(testStreetcode, new List<RelatedFigure>(), -1);

            // Act
            var result = await _handler.Handle(new DeleteStreetcodeCommand(id), CancellationToken.None);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorValue, result.Errors.Single().Message);
            _mockLogger.Verify(logger => logger.LogError(It.IsAny<DeleteStreetcodeCommand>(), It.IsAny<string>()), Times.Once);
            _repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        private void SetupRepositoryMocks(StreetcodeContent? streetcodeContent, List<RelatedFigure> relatedFigures, int saveChangesVariable)
        {
            _repositoryMock
                .Setup(repo => repo.StreetcodeRepository.GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<StreetcodeContent, bool>>>(),
                    It.IsAny<Func<IQueryable<StreetcodeContent>, IIncludableQueryable<StreetcodeContent, object>>>()))
                .ReturnsAsync(streetcodeContent);

            _repositoryMock
                .Setup(repo => repo.RelatedFigureRepository.GetAllAsync(It.IsAny<Expression<Func<RelatedFigure, bool>>>(), null))
                .ReturnsAsync(relatedFigures);

            if (streetcodeContent != null)
            {
                _repositoryMock.Setup(repo => repo.StreetcodeRepository.Delete(streetcodeContent));
            }

            _repositoryMock
                .Setup(repo => repo.SaveChangesAsync())
                .ReturnsAsync(saveChangesVariable);
        }
    }
}
