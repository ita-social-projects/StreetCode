using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
    public class UpdateTermHendlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerCannotConvertNull;

        public UpdateTermHendlerTests()
        {
            this._mockRepository = new ();
            this._mockMapper = new ();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this._mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNuber)
        {
            // Arrange
            this._mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
            .Returns(GetTerm());

            var handler = new UpdateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsSuccess),
                () => Assert.IsType<Unit>(result.Value));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldThrowExeption_TryMapNullRequest(int returnNuber)
        {
            // Arrange
            this._mockRepository.Setup(x => x.TermRepository.Update(GetTermWithNotExistId() !));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
                .Returns(GetTermWithNotExistId() !);

            var expectedError = "Cannot convert null to Term";
            this._mockLocalizerCannotConvertNull.Setup(x => x["CannotConvertNullToTerm"])
                .Returns(new LocalizedString("CannotConvertNullToTerm", expectedError));

            var handler = new UpdateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateTermCommand(GetTermDTOWithNotExistId() !), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.False(result.IsSuccess),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(-1)]
        public async Task ShouldThrowExeption_SaveChangesAsyncIsNotSuccessful(int returnNuber)
        {
            // Arrange
            this._mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
                .Returns(GetTerm());

            var expectedError = "Failed to update a term";
            this._mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateTerm"])
               .Returns(new LocalizedString("FailedToUpdateTerm", expectedError));
            var handler = new UpdateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(expectedError, result.Errors[0].Message));
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_TypeIsCorrect(int returnNuber)
        {
            // Arrange
            this._mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDTO>()))
                .Returns(GetTerm());

            var handler = new UpdateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerFailedToUpdate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<Unit>(result.Value);
        }

        private static Term GetTerm() => new ();

        private static TermDTO GetTermDTO() => new ();

        private static Term? GetTermWithNotExistId() => null;

        private static TermDTO? GetTermDTOWithNotExistId() => null;
    }
}
