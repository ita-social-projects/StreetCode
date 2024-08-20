using System.Linq.Expressions;
using AutoMapper;
using FluentResults;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;
using Entity = Streetcode.DAL.Entities.Streetcode.TextContent.RelatedTerm;
using EntityDTO = Streetcode.BLL.DTO.Streetcode.TextContent.RelatedTermDTO;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.RelatedTerm.Delete
{
    public class DeleteRelatedTermHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotFindSharedResource>> _mockLocalizerCannotFind;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> _mockLocalizerFailedToDelete;

        public DeleteRelatedTermHandlerTests()
        {
            this._repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            this._mapperMock = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotFind = new Mock<IStringLocalizer<CannotFindSharedResource>>();
            this._mockLocalizerFailedToDelete = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
        }

        [Theory]
        [InlineData(1, "test", 1)]
        public async Task Handle_WhenRelatedTermExists_DeletesItAndReturnsSuccessResult(int id, string word, int termId)
        {
            // Arrange
            var relatedTerm = CreateNewEntity(id, word, termId);
            this._repositoryWrapperMock
                .Setup(r => r.RelatedTermRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerm);
            this._repositoryWrapperMock.Setup(r => r.RelatedTermRepository.Delete(relatedTerm));
            this._repositoryWrapperMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            var handler = new DeleteRelatedTermHandler(this._repositoryWrapperMock.Object, this._mapperMock.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this._mockLocalizerCannotFind.Object);
            var command = new DeleteRelatedTermCommand(word);
            this.SetupLocalizers();

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            this._repositoryWrapperMock.Verify(r => r.RelatedTermRepository.Delete(relatedTerm), Times.Once);
            this._repositoryWrapperMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.Multiple(
                () => Assert.NotNull(result));
        }

        [Theory]
        [InlineData("word")]
        public async Task Handle_WhenRelatedTermDoesNotExist_ReturnsFailedResult(string word)
        {
            // Arrange
            this._repositoryWrapperMock
                .Setup(r => r.RelatedTermRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync((Entity?)null);
            var sut = new DeleteRelatedTermHandler(this._repositoryWrapperMock.Object, this._mapperMock.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this._mockLocalizerCannotFind.Object);
            var command = new DeleteRelatedTermCommand(word);
            var expectedError = $"Cannot find a related term: {word}";
            this.SetupLocalizers();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<EntityDTO>>(result),
                () => Assert.False(result.IsSuccess),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(1, "Test Word", 1)]
        public async Task Handle_WhenDeletionFails_ReturnsFailedResult(int id, string word, int termId)
        {
            // Arrange
            var relatedTerm = CreateNewEntity(id, word, termId);
            this._repositoryWrapperMock
                .Setup(r => r.RelatedTermRepository
                    .GetFirstOrDefaultAsync(
                        It.IsAny<Expression<Func<Entity, bool>>>(),
                        It.IsAny<Func<IQueryable<Entity>,
                        IIncludableQueryable<Entity, object>>>()))
                .ReturnsAsync(relatedTerm);
            this._repositoryWrapperMock.Setup(rw => rw.RelatedTermRepository.Delete(It.IsAny<Entity>()));
            this._repositoryWrapperMock.Setup(rw => rw.SaveChangesAsync())
                 .ReturnsAsync(0);
            var sut = new DeleteRelatedTermHandler(this._repositoryWrapperMock.Object, this._mapperMock.Object, this._mockLogger.Object, this._mockLocalizerFailedToDelete.Object, this._mockLocalizerCannotFind.Object);
            var command = new DeleteRelatedTermCommand(word);
            var expectedError = "Failed to delete a related term";
            this.SetupLocalizers();

            // Act
            var result = await sut.Handle(command, CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.IsType<Result<EntityDTO>>(result),
                () => Assert.False(result.IsSuccess),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        private static Entity CreateNewEntity(int id, string word, int termId)
        {
            return new Entity { Id = id, Word = word, TermId = termId };
        }

        private void SetupLocalizers()
        {
            // Setup for _mockLocalizerCannotFind
            this._mockLocalizerCannotFind.Setup(x => x[It.IsAny<string>(), It.IsAny<object>()])
                .Returns((string key, object[] args) =>
                {
                    if (args != null && args.Length > 0 && args[0] is string word)
                    {
                        return new LocalizedString(key, $"Cannot find a related term: {word}");
                    }

                    return new LocalizedString(key, "Cannot find any related term with unknown word");
                });

            // Setup for _mockLocalizerFailedToDelete
            this._mockLocalizerFailedToDelete
                .Setup(x => x["FailedToDeleteRelatedTerm"])
                .Returns(new LocalizedString("FailedToDeleteRelatedTerm", "Failed to delete a related term"));
        }
    }
}
