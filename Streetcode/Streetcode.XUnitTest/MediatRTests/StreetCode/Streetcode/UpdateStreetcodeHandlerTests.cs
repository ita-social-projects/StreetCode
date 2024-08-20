using AutoMapper;
using FluentResults;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Interfaces.Cache;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Update;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.StreetCode.Streetcode
{
    public class UpdateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _repository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> _mockLocalizerAnErrorOccurred;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> _mockLocalizerFailedToUpdate;
        private readonly Mock<ICacheService> _mockCache;

        public UpdateStreetcodeHandlerTests()
        {
            this._repository = new Mock<IRepositoryWrapper>();
            this._mapper = new Mock<IMapper>();
            this._mockLogger = new Mock<ILoggerService>();
            this._mockLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            this._mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this._mockCache = new Mock<ICacheService>();
            this._mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDTO>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDTO>>>>, TimeSpan>((key, func, timeSpan) =>
                {
                    return func();
                });
        }

        [Fact]
        public async Task Handle_ReturnsSuccess()
        {
            // Arrange
            var testStreetcode = new StreetcodeContent();
            var testStreetcodeDTO = new StreetcodeUpdateDTO();
            string expectedErrorMessage = "An error occurred while updating a streetcode";
            int testSaveChangesSuccess = 1;

            this.RepositorySetup(testStreetcode, testSaveChangesSuccess);
            this.MapperSetup(testStreetcode);

            this._mockLocalizerAnErrorOccurred.Setup(x => x["AnErrorOccurredWhileUpdatin"])
                .Returns(new LocalizedString("AnErrorOccurredWhileUpdatin", expectedErrorMessage));

            var handler =
                new UpdateStreetcodeHandler(
                    this._mapper.Object,
                    this._repository.Object,
                    this._mockLogger.Object,
                    this._mockLocalizerAnErrorOccurred.Object,
                    this._mockLocalizerFailedToUpdate.Object,
                    this._mockCache.Object);

            // Act
            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message),
                () => Assert.False(result.IsSuccess));
        }

        private void RepositorySetup(StreetcodeContent testStreetcode, int saveChangesVariable)
        {
            this._repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            this._repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
        }

        private void MapperSetup(StreetcodeContent? testStreetcode)
        {
            this._mapper.Setup(x => x.Map<StreetcodeContent?>(It.IsAny<object>())).Returns(testStreetcode);
        }
    }
}