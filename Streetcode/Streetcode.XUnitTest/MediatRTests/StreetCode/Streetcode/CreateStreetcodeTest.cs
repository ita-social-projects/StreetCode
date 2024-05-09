using Microsoft.Extensions.Localization;
using Moq;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.DAL.Entities.Streetcode;
using System;
using System.Net;
using System.Net.Http;
using Xunit;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Repositories.Interfaces.Base;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;

namespace Streetcode.BLL.MediatR.Streetcode.Streetcode
{
    public class CreateStreetcodeHandlerTests
    {
        private readonly Mock<IRepositoryWrapper> _mockRepositoryWrapper;
        private readonly Mock<IStringLocalizer<FailedToCreateSharedResource>> _mockStringLocalizerFailedToCreate;
        private readonly Mock<IStringLocalizer<AnErrorOccurredSharedResource>> _mockStringLocalizerAnErrorOccurred;

        public CreateStreetcodeHandlerTests()
        {
            _mockRepositoryWrapper = new Mock<IRepositoryWrapper>();
            _mockStringLocalizerFailedToCreate = new Mock<IStringLocalizer<FailedToCreateSharedResource>>();
            _mockStringLocalizerAnErrorOccurred = new Mock<IStringLocalizer<AnErrorOccurredSharedResource>>();
        }

        [Fact]
        public void AddTextContent_TitleNotEmpty_TextContentEmpty_NoExceptionThrown()
        {
            // Arrange
            var textContent = new TextCreateDTO { TextContent = "test" };
            var streetcode = new StreetcodeContent { Title = "" };
            var handler = new CreateStreetcodeHandler(null, null, null, null, null);

            // Act & Assert
            Assert.Throws<HttpRequestException>(() => handler.AddTextContent(textContent, streetcode));
        }
    }
}
