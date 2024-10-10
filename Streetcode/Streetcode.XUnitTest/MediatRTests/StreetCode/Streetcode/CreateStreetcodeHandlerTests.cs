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
using Streetcode.BLL.DTO.Streetcode.Create;
using Xunit;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create.Tests
{
    public class CreateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> _mockStringLocalizerAnErrorOccurred;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockStringLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<FailedToValidateSharedResource>> _mockStringLocalizerFailedToValidate;
        private readonly Mock<IStringLocalizer<FieldNamesSharedResource>> _mockStringLocalizerFieldNames;
        private readonly Mock<IMapper> _mockMapper;

        public CreateStreetcodeHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLoggerService = new Mock<ILoggerService>();
            _mockMapper = new Mock<IMapper>();
            _mockStringLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            _mockStringLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockStringLocalizerFailedToValidate = new Mock<IStringLocalizer<FailedToValidateSharedResource>>();
            _mockStringLocalizerFieldNames = new Mock<IStringLocalizer<FieldNamesSharedResource>>();
        }

        private void SetupMockStreetcodeCreate()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StreetcodeRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeContent>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockStreetcodeImageCreate()
        {
            _mockRepositoryWrapper.Setup(repo => repo.StreetcodeImageRepository.CreateRangeAsync(It.IsAny<IEnumerable<StreetcodeImage>>()))
                .Returns(Task.CompletedTask);
        }

        private void SetupMockStreetcodeImageDetailsCreate()
        {
            _mockRepositoryWrapper.Setup(repo => repo.ImageDetailsRepository.CreateRangeAsync(It.IsAny<IEnumerable<ImageDetails>>()))
                .Returns(Task.CompletedTask);
        }

        [Fact]
        public async Task AddImagesDetails_ValidImageDetails_DoesNotThrowException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = "Alt2" }
            };
            SetupMockStreetcodeImageDetailsCreate();
            var handler = new CreateStreetcodeHandler(
                _mockMapper.Object,
                _mockRepositoryWrapper.Object,
                _mockLoggerService.Object,
                _mockStringLocalizerAnErrorOccurred.Object,
                _mockStringLocalizerFailedToCreate.Object,
                _mockStringLocalizerFailedToValidate.Object,
                _mockStringLocalizerFieldNames.Object);

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
            SetupMockStreetcodeImageCreate();

            var handler = new CreateStreetcodeHandler(
                _mockMapper.Object,
                _mockRepositoryWrapper.Object,
                _mockLoggerService.Object,
                _mockStringLocalizerAnErrorOccurred.Object,
                _mockStringLocalizerFailedToCreate.Object,
                _mockStringLocalizerFailedToValidate.Object,
                _mockStringLocalizerFieldNames.Object);

            // Act & Assert
            Func<Task> action = async () => await handler.AddImagesAsync(streetcode, imagesIds);
            await action.Should().NotThrowAsync<HttpRequestException>();
        }
    }
}
