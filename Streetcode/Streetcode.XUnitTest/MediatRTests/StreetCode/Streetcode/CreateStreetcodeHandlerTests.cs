using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Xunit;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create.Tests
{
    public class CreateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> mockRepositoryWrapper;
        private readonly Mock<ILoggerService> mockLoggerService;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> mockStringLocalizerAnErrorOccurred;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> mockStringLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> mockStringLocalizerFailedToValidate;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> mockStringLocalizerFieldNames;
        private readonly Mock<IMapper> mockMapper;

        public CreateStreetcodeHandlerTests()
        {
            this.mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            this.mockLoggerService = new Mock<ILoggerService>();
            this.mockMapper = new Mock<IMapper>();
            this.mockStringLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            this.mockStringLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            this.mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            this.mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        }

        [Fact]
        public async Task AddImagesDetails_ValidImageDetails_DoesNotThrowException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = "Alt2" },
            };
            this.SetupMockStreetcodeImageDetailsCreate();
            var handler = new CreateStreetcodeHandler(
                this.mockMapper.Object,
                this.mockRepositoryWrapper.Object,
                this.mockLoggerService.Object,
                this.mockStringLocalizerAnErrorOccurred.Object,
                this.mockStringLocalizerFailedToCreate.Object,
                this.mockStringLocalizerFailedToValidate.Object,
                this.mockStringLocalizerFieldNames.Object);

            // Act & Assert
            Func<Task> action = async () => await handler.AddImagesDetails(imageDetails);
            await action.Should().NotThrowAsync<HttpRequestException>();
        }

        [Fact]
        public async Task AddImagesAsync_ValidImagesIds_DoesNotThrowException()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int> imagesIds = new List<int> { 1, 2, 3 };
            this.SetupMockStreetcodeImageCreate();

            var handler = new CreateStreetcodeHandler(
                this.mockMapper.Object,
                this.mockRepositoryWrapper.Object,
                this.mockLoggerService.Object,
                this.mockStringLocalizerAnErrorOccurred.Object,
                this.mockStringLocalizerFailedToCreate.Object,
                this.mockStringLocalizerFailedToValidate.Object,
                this.mockStringLocalizerFieldNames.Object);

            // Act & Assert
            Func<Task> action = async () => await handler.AddImagesAsync(streetcode, imagesIds);
            await action.Should().NotThrowAsync<HttpRequestException>();
        }

        private void SetupMockStreetcodeImageCreate()
        {
            this.mockRepositoryWrapper.Setup(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockStreetcodeImageDetailsCreate()
        {
            this.mockRepositoryWrapper.Setup(repo => repo.ImageDetailsRepository.CreateRangeAsync(It.IsAny<IEnumerable<ImageDetails>>()))
                .Returns(Task.CompletedTask);
        }
    }
}
