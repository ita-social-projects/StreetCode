using AutoMapper;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Term.Create;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Terms
{
    public class CreateTermHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> _mockLocalizerCannotCreate;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> _mockLocalizerCannotConvertNull;

        public CreateTermHandlerTests()
        {
            this._mockMapper = new ();
            this._mockRepository = new ();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this._mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this._mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenTermAdded(int returnNumber)
        {
            // Arrange
            var createdTerm = GetTerm();
            this._mockRepository.Setup(x => x.TermRepository.Create(It.IsAny<Term>())).Returns(createdTerm);
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermCreateDTO>())).Returns(createdTerm);
            this._mockMapper.Setup(x => x.Map<TermDTO>(createdTerm)).Returns(GetTermDTO());

            var handler = new CreateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await handler.Handle(new CreateTermCommand(GetTermCreateDTO()), CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldThrowException_WhenTryToAddNull(int returnNumber)
        {
            // Arrange
            this._mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
            this._mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

            this._mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermCreateDTO>()))
              .Returns(GetNotExistingTerm() !);

            var expectedError = "Cannot convert null to Term";
            this._mockLocalizerCannotConvertNull.Setup(x => x["CannotConvertNullToTerm"])
                .Returns(new LocalizedString("CannotConvertNullToTerm", expectedError));
            var hendler = new CreateTermHandler(this._mockMapper.Object, this._mockRepository.Object, this._mockLogger.Object, this._mockLocalizerCannotCreate.Object, this._mockLocalizerFailedToCreate.Object, this._mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await hendler.Handle(new CreateTermCommand(GetNotExistingTermDTO() !), CancellationToken.None);

            // Assert
            Assert.Multiple(
              () => Assert.Equal(expectedError, result.Errors[0].Message),
              () => Assert.False(result.IsSuccess));
        }

        private static Term GetTerm() => new ();

        private static TermCreateDTO GetTermCreateDTO() => new ();

        private static TermDTO GetTermDTO() => new ();

        private static Term? GetNotExistingTerm() => null;

        private static TermCreateDTO? GetNotExistingTermDTO() => null;
    }
}
