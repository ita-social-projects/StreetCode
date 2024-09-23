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
        private readonly Mock<IMapper> _mockMapper;
        
        public CreateStreetcodeHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLoggerService = new Mock<ILoggerService>();
            _mockMapper = new Mock<IMapper>();
            _mockStringLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            _mockStringLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
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
        public async Task AddImagesDetails_NullImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            IEnumerable<ImageDetailsDto>? imageDetails = null;
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesDetails(imageDetails));
        }

        [Fact]
        public async Task AddImagesDetails_EmptyImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            IEnumerable<ImageDetailsDto> imageDetails = new List<ImageDetailsDto>();
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesDetails(imageDetails));
        }

        [Fact]
        public async Task AddImagesDetails_NullAltInImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = null }
            };
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesDetails(imageDetails));
        }

        [Fact]
        public async Task AddImagesDetails_EmptyAltInImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = string.Empty }
            };
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesDetails(imageDetails));
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
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            Func<Task> action = async () => await handler.AddImagesDetails(imageDetails);
            await action.Should().NotThrowAsync<HttpRequestException>();
        }
        [Fact]
        public async Task AddImagesAsync_NullImagesIds_ThrowsHttpRequestException()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int> imagesIds = null;
            SetupMockStreetcodeImageCreate();
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesAsync(streetcode, imagesIds));
        }

        [Fact]
        public async Task AddImagesAsync_EmptyImagesIds_ThrowsHttpRequestException()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int> imagesIds = new List<int>();
            SetupMockStreetcodeImageCreate();
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => handler.AddImagesAsync(streetcode, imagesIds));
        }

        [Fact]
        public async Task AddImagesAsync_ValidImagesIds_DoesNotThrowException()
        {
            // Arrange
            StreetcodeContent streetcode = new StreetcodeContent();
            IEnumerable<int> imagesIds = new List<int> { 1, 2, 3 };
            SetupMockStreetcodeImageCreate();

            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                                                      _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act & Assert
            Func<Task> action = async () => await handler.AddImagesAsync(streetcode, imagesIds);
            await action.Should().NotThrowAsync<HttpRequestException>();
        }
        
        [Theory]
        [InlineData(-5)]
        [InlineData(10000000)]
        public async Task Handle_IndexIsOutOfBounds_ReturnsFailure(int index)
        {
            // Arrange
            StreetcodeCreateDTO streetcode = new StreetcodeCreateDTO() { Index = index };
            SetupMockStreetcodeCreate();
            var command = new CreateStreetcodeCommand(streetcode);
            var handler = new CreateStreetcodeHandler(_mockMapper.Object, _mockRepositoryWrapper.Object, _mockLoggerService.Object,
                _mockStringLocalizerAnErrorOccurred.Object, _mockStringLocalizerFailedToCreate.Object);

            // Act
            var result = await handler.Handle(command, default);

            // Assert
            Assert.Multiple(
                () => Assert.True(result.IsFailed),
                () => Assert.Equal(
                    $"The 'index' must be in range from {CreateStreetcodeHandler.StreetcodeIndexMinValue} to {CreateStreetcodeHandler.StreetcodeIndexMaxValue}.",
                    result.Errors.Single().Message));
        }
    }
}
