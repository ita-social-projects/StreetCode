using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using Moq.Protected;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.DTO.ReCaptchaResponseDTO;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Email;
using Streetcode.DAL.Entities.AdditionalContent.Email;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Email
{
    public class SendEmailHandlerTests
    {
        private readonly Mock<IEmailService> mockEmailService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<SendEmailHandler>> mockStringLocalizer;
        private readonly Mock<HttpClient> mockHttpClient;
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;
        private readonly Mock<IConfiguration> mockConfiguration;

        public SendEmailHandlerTests()
        {
            this.mockEmailService = new Mock<IEmailService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockStringLocalizer = new Mock<IStringLocalizer<SendEmailHandler>>();
            this.mockHttpClient = new Mock<HttpClient>();
            this.mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            this.mockConfiguration = new Mock<IConfiguration>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_EmailIsCorrect()
        {
            // arrange
            var reCaptchaResponseDto = GetReCaptchaResponseDTO();
            var emailDto = GetEmailDTO();

            this.SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            this.SetupMockEmailServiceReturnsOK();

            var client = new HttpClient(this.mockHttpMessageHandler.Object);

            var handler = new SendEmailHandler(this.mockEmailService.Object, this.mockLogger.Object, this.mockStringLocalizer.Object, client, this.mockConfiguration.Object);

            // act
            var result = await handler.Handle(new SendEmailCommand(emailDto), CancellationToken.None);

            // assert
            Assert.True(result.IsSuccess);
        }

        private void SetupMockHttpMessageHandlerReturnsOK(ReCaptchaResponseDto reCaptchaResponseDto)
        {
            this.mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = JsonContent.Create<ReCaptchaResponseDto>(reCaptchaResponseDto) });
        }

        private void SetupMockEmailServiceReturnsOK()
        {
            this.mockEmailService
                .Setup<Task<bool>>(service => service.SendEmailAsync(It.IsAny<Message>()))
                .Returns(Task.FromResult(true));
        }

        private static ReCaptchaResponseDto GetReCaptchaResponseDTO()
        {
            return new ReCaptchaResponseDto()
            {
                Success = true,
            };
        }

        private static EmailDTO GetEmailDTO()
        {
            return new EmailDTO()
            {
                Content = "Test Content",
                From = "testemail@gmail.com",
                Token = "Token",
            };
        }
    }
}
