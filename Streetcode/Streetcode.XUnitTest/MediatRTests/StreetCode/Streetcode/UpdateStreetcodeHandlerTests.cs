using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Http;
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
        private readonly Mock<IRepositoryWrapper> repository;
        private readonly Mock<IMapper> mapper;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> mockLocalizerAnErrorOccurred;
        private readonly Mock<IStringLocalizer<FailedToUpdateSharedResource>> mockLocalizerFailedToUpdate;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockStringLocalizerFailedToValidate;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockStringLocalizerFieldNames;
        private readonly Mock<ICacheService> mockCache;
        private readonly Mock<IHttpContextAccessor> mockHttpContextAccessor;

        public UpdateStreetcodeHandlerTests()
        {
            this.repository = new Mock<IRepositoryWrapper>();
            this.mapper = new Mock<IMapper>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            this.mockLocalizerFailedToUpdate = new Mock<IStringLocalizer<FailedToUpdateSharedResource>>();
            this.mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            this.mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
            this.mockCache = new Mock<ICacheService>();
            this.mockCache
                .Setup(c => c.GetOrSetAsync(It.IsAny<string>(), It.IsAny<Func<Task<Result<IEnumerable<ImageDTO>>>>>(), It.IsAny<TimeSpan>()))
                .Returns<string, Func<Task<Result<IEnumerable<ImageDTO>>>>, TimeSpan>((key, func, timeSpan) =>
                {
                    return func();
                });
            this.mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
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

            this.mockLocalizerAnErrorOccurred.Setup(x => x["AnErrorOccurredWhileUpdating", It.IsAny<object[]>()])
                .Returns(new LocalizedString("AnErrorOccurredWhileUpdating", expectedErrorMessage));

            var handler =
                new UpdateStreetcodeHandler(
                    this.mapper.Object,
                    this.repository.Object,
                    this.mockLogger.Object,
                    this.mockLocalizerAnErrorOccurred.Object,
                    this.mockLocalizerFailedToUpdate.Object,
                    this.mockStringLocalizerFailedToValidate.Object,
                    this.mockStringLocalizerFieldNames.Object,
                    this.mockCache.Object,
                    this.mockHttpContextAccessor.Object);

            // Act
            var result = await handler.Handle(new UpdateStreetcodeCommand(testStreetcodeDTO), CancellationToken.None);

            // Assert
            Assert.Multiple(
                () => Assert.Equal(expectedErrorMessage, result.Errors[0].Message),
                () => Assert.False(result.IsSuccess));
        }

        private void RepositorySetup(StreetcodeContent testStreetcode, int saveChangesVariable)
        {
            this.repository.Setup(x => x.StreetcodeRepository.Update(testStreetcode));
            this.repository.Setup(x => x.SaveChangesAsync()).ReturnsAsync(saveChangesVariable);
        }

        private void MapperSetup(StreetcodeContent? testStreetcode)
        {
            this.mapper.Setup(x => x.Map<StreetcodeContent?>(It.IsAny<object>())).Returns(testStreetcode);
        }
    }
}