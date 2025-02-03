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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerCannotConvertNull;

        public UpdateTermHendlerTests()
        {
            this.mockRepository = new ();
            this.mockMapper = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this.mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenUpdated(int returnNuber)
        {
            // Arrange
            this.mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDto>()))
            .Returns(GetTerm());

            var handler = new UpdateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerCannotConvertNull.Object);

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
            this.mockRepository.Setup(x => x.TermRepository.Update(GetTermWithNotExistId() !));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDto>()))
                .Returns(GetTermWithNotExistId() !);

            var expectedError = "Cannot convert null to Term";
            this.mockLocalizerCannotConvertNull.Setup(x => x["CannotConvertNullToTerm"])
                .Returns(new LocalizedString("CannotConvertNullToTerm", expectedError));

            var handler = new UpdateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerCannotConvertNull.Object);

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
            this.mockRepository.Setup(x => x.TermRepository.Update(GetTerm()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDto>()))
                .Returns(GetTerm());

            var expectedError = "Failed to update a term";
            this.mockLocalizerFailedToUpdate.Setup(x => x["FailedToUpdateTerm"])
               .Returns(new LocalizedString("FailedToUpdateTerm", expectedError));
            var handler = new UpdateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerCannotConvertNull.Object);

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
            this.mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNuber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermDto>()))
                .Returns(GetTerm());

            var handler = new UpdateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerFailedToUpdate.Object, this.mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new UpdateTermCommand(GetTermDTO()), CancellationToken.None);

            // Assert
            Assert.IsType<Unit>(result.Value);
        }

        private static Term GetTerm() => new ();

        private static TermDto GetTermDTO() => new ();

        private static Term? GetTermWithNotExistId() => null;

        private static TermDto? GetTermDTOWithNotExistId() => null;
    }
}
