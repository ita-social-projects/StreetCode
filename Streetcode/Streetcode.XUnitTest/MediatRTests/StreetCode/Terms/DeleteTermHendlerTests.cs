using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Delete;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;

using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
    public class DeleteTermHendlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToDeleteSharedResource>> mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerCannotConvertNull;

        public DeleteTermHendlerTests()
        {
            this.mockRepository = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToDeleteSharedResource>>();
            this.mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Theory]
        [InlineData(-1, 1)]
        public async Task ShouldDeleteSuccessfully(int id, int returnNuber)
        {
            // Arrange
            MockRepoInitial_GetFirstOrDefault_Delete(this.mockRepository, id, true);
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);
            var handler = new DeleteTermHandler(this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new DeleteTermCommand(id), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.NotNull(result),
                () => Assert.True(result.IsSuccess));
        }

        [Theory]
        [InlineData(2)]
        public async Task ShouldThrowExeption_IdNotExisting(int id)
        {
            // Arrange
            MockRepoInitial_GetFirstOrDefault_Delete(this.mockRepository, id, false);

            var expectedError = "Cannot convert null to Term";
            this.mockLocalizerCannotConvertNull.Setup(x => x["CannotConvertNullToTerm"])
                .Returns(new LocalizedString("CannotConvertNullToTerm", expectedError));

            var handler = new DeleteTermHandler(this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new DeleteTermCommand(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        [Theory]
        [InlineData(2, 0)]
        public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int id, int returnNuber)
        {
            // Arange
            MockRepoInitial_GetFirstOrDefault_Delete(this.mockRepository, id, true);
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            var expectedError = "Failed to delete a term";
            this.mockLocalizerFailedToCreate.Setup(x => x["FailedToDeleteTerm"])
               .Returns(new LocalizedString("FailedToDeleteTerm", expectedError));
            var hendler = new DeleteTermHandler(this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await hendler.Handle(new DeleteTermCommand(id), CancellationToken.None);

            // Assert
            Assert.Equal(expectedError, result.Errors[0].Message);
        }

        private static void MockRepoInitial_GetFirstOrDefault_Delete(
            Mock<IRepositoryWrapper> mockRepo, int id, bool isIdExist)
        {
            mockRepo.Setup(x => x.TermRepository
                .GetFirstOrDefaultAsync(
                    It.IsAny<Expression<Func<Term, bool>>>(),
                    It.IsAny<Func<IQueryable<Term>,
                        IIncludableQueryable<Term, object>>>()))
                .ReturnsAsync(isIdExist ? GetTerm(id) : GetNotExistingTerm() !);

            mockRepo.Setup(x => x.TermRepository.Delete(isIdExist ? GetTerm(id) : GetNotExistingTerm() !));
        }

        private static Term GetTerm(int id) => new () { Id = id };

        private static Term? GetNotExistingTerm() => null;
    }
}
