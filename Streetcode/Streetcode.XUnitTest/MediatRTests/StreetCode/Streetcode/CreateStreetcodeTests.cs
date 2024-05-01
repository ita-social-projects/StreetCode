using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode.Create.Tests
{
    public class CreateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<ILoggerService> _mockLoggerService;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> _mockStringLocalizerAnErrorOccurred;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockStringLocalizerFailedToCreate;

        public CreateStreetcodeHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockLoggerService = new Mock<ILoggerService>();
            _mockStringLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            _mockStringLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
        }

        [Fact]
        public void AddImagesDetails_NullImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            IEnumerable<ImageDetailsDto>? imageDetails = null;
            var mapperMock = new Mock<IMapper>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var stringLocalizerAnErrorOccurredMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            var stringLocalizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            var handler = new CreateStreetcodeHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerServiceMock.Object,
                                                      stringLocalizerAnErrorOccurredMock.Object, stringLocalizerFailedToCreateMock.Object);

            // Act
            Action action = () => handler.AddImagesDetails(imageDetails);

            // Assert
            action.Should().Throw<HttpRequestException>()
                .WithMessage("There is no valid imagesDetails value")
                .And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void AddImagesDetails_EmptyImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            IEnumerable<ImageDetailsDto> imageDetails = new List<ImageDetailsDto>();
            var mapperMock = new Mock<IMapper>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var stringLocalizerAnErrorOccurredMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            var stringLocalizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            var handler = new CreateStreetcodeHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerServiceMock.Object,
                                                      stringLocalizerAnErrorOccurredMock.Object, stringLocalizerFailedToCreateMock.Object);

            // Act
            Action action = () => handler.AddImagesDetails(imageDetails);

            // Assert
            action.Should().Throw<HttpRequestException>()
                .WithMessage("There is no valid imagesDetails value")
                .And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void AddImagesDetails_NullAltInImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = null }
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var stringLocalizerAnErrorOccurredMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            var stringLocalizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            var handler = new CreateStreetcodeHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerServiceMock.Object,
                                                      stringLocalizerAnErrorOccurredMock.Object, stringLocalizerFailedToCreateMock.Object);

            // Act
            Action action = () => handler.AddImagesDetails(imageDetails);

            // Assert
            action.Should().Throw<HttpRequestException>()
                .WithMessage("There is no valid imagesDetails value")
                .And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void AddImagesDetails_EmptyAltInImageDetails_ThrowsHttpRequestException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = string.Empty }
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var stringLocalizerAnErrorOccurredMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            var stringLocalizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            var handler = new CreateStreetcodeHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerServiceMock.Object,
                                                      stringLocalizerAnErrorOccurredMock.Object, stringLocalizerFailedToCreateMock.Object);

            // Act
            Action action = () => handler.AddImagesDetails(imageDetails);

            // Assert
            action.Should().Throw<HttpRequestException>()
                .WithMessage("There is no valid imagesDetails value")
                .And.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public void AddImagesDetails_ValidImageDetails_DoesNotThrowException()
        {
            // Arrange
            var imageDetails = new List<ImageDetailsDto>
            {
                new ImageDetailsDto { Alt = "Alt1" },
                new ImageDetailsDto { Alt = "Alt2" }
            };
            var mapperMock = new Mock<IMapper>();
            var repositoryWrapperMock = new Mock<IRepositoryWrapper>();
            var loggerServiceMock = new Mock<ILoggerService>();
            var stringLocalizerAnErrorOccurredMock = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
            var stringLocalizerFailedToCreateMock = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            var handler = new CreateStreetcodeHandler(mapperMock.Object, repositoryWrapperMock.Object, loggerServiceMock.Object,
                                                      stringLocalizerAnErrorOccurredMock.Object, stringLocalizerFailedToCreateMock.Object);

            // Act
            Action action = () => handler.AddImagesDetails(imageDetails);

            // Assert
            action.Should().NotThrow<HttpRequestException>();
        }
    }
}
