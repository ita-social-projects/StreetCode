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
        private readonly Mock<IRepositoryWrapper> mockRepository;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<CannotCreateSharedResource>> mockLocalizerCannotCreate;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<CannotConvertNullSharedResource>> mockLocalizerCannotConvertNull;

        public CreateTermHandlerTests()
        {
            this.mockMapper = new ();
            this.mockRepository = new ();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerCannotConvertNull = new Mock<IStringLocalizer<CannotConvertNullSharedResource>>();
            this.mockLocalizerCannotCreate = new Mock<IStringLocalizer<CannotCreateSharedResource>>();
            this.mockLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
        }

        [Theory]
        [InlineData(1)]
        public async Task ShouldReturnSuccessfully_WhenTermAdded(int returnNumber)
        {
            // Arrange
            var createdTerm = GetTerm();
            this.mockRepository.Setup(x => x.TermRepository.Create(It.IsAny<Term>())).Returns(createdTerm);
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermCreateDto>())).Returns(createdTerm);
            this.mockMapper.Setup(x => x.Map<TermDto>(createdTerm)).Returns(GetTermDTO());

            var handler = new CreateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerCannotConvertNull.Object);

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
            this.mockRepository.Setup(x => x.TermRepository.Create(GetTerm()));
            this.mockRepository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(returnNumber);

            this.mockMapper.Setup(x => x.Map<Term>(It.IsAny<TermCreateDto>()))
              .Returns(GetNotExistingTerm() !);

            var expectedError = "Cannot convert null to Term";
            this.mockLocalizerCannotConvertNull.Setup(x => x["CannotConvertNullToTerm"])
                .Returns(new LocalizedString("CannotConvertNullToTerm", expectedError));
            var hendler = new CreateTermHandler(this.mockMapper.Object, this.mockRepository.Object, this.mockLogger.Object, this.mockLocalizerCannotCreate.Object, this.mockLocalizerFailedToCreate.Object, this.mockLocalizerCannotConvertNull.Object);

            // Act
            var result = await hendler.Handle(new CreateTermCommand(GetNotExistingTermDTO() !), CancellationToken.None);

            // Assert
            Assert.Multiple(
              () => Assert.Equal(expectedError, result.Errors[0].Message),
              () => Assert.False(result.IsSuccess));
        }

        private static Term GetTerm() => new ();

        private static TermCreateDto GetTermCreateDTO() => new ();

        private static TermDto GetTermDTO() => new ();

        private static Term? GetNotExistingTerm() => null;

        private static TermCreateDto? GetNotExistingTermDTO() => null;
    }
}
