using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Moq;
using Moq.Protected;
using Streetcode.BLL.DTO.Email;
using Streetcode.BLL.DTO.ReCaptchaResponseDTO;
using Streetcode.BLL.Factories.MessageDataFactory.Abstracts;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.MediatR.Email;
using Streetcode.BLL.Models.Email.Messages.Base;
using Xunit;

namespace Streetcode.XUnitTest.MediatRTests.Email
{
    public class SendEmailHandlerTests
    {
        private readonly Mock<IEmailService> mockEmailService;
        private readonly Mock<ILoggerService> mockLogger;
        private readonly Mock<IStringLocalizer<SendEmailHandler>> mockStringLocalizer;
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler;
        private readonly Mock<IConfiguration> mockConfiguration;
        private readonly Mock<IMessageDataAbstractFactory> mockMessageDataAbstractFactory;
        private readonly Mock<IHttpClientFactory> mockHttpClientFactory;

        public SendEmailHandlerTests()
        {
            this.mockEmailService = new Mock<IEmailService>();
            this.mockLogger = new Mock<ILoggerService>();
            this.mockStringLocalizer = new Mock<IStringLocalizer<SendEmailHandler>>();
            this.mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            this.mockConfiguration = new Mock<IConfiguration>();
            this.mockMessageDataAbstractFactory = new Mock<IMessageDataAbstractFactory>();
            this.mockStringLocalizer.Setup(x => x["RecaptchaRequestFailed"]).Returns(new LocalizedString("RecaptchaRequestFailed", "RecaptchaRequestFailed"));
            this.mockStringLocalizer.Setup(x => x["InvalidCaptcha"]).Returns(new LocalizedString("InvalidCaptcha", "InvalidCaptcha"));
            this.mockHttpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_EmailIsCorrect()
        {
            // arrange
            var reCaptchaResponseDto = GetReCaptchaResponseDTO(true);
            var emailDto = GetEmailDTO();

            this.SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            this.SetupMockEmailServiceReturnsOK();
            this.SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(this.mockEmailService.Object, this.mockLogger.Object, this.mockStringLocalizer.Object, this.mockHttpClientFactory.Object, this.mockConfiguration.Object, this.mockMessageDataAbstractFactory.Object);

            // act
            var result = await handler.Handle(new SendEmailCommand(emailDto), CancellationToken.None);

            // assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ShouldReturnFail_WhenHttpClientRequestFailed()
        {
            // arrange
            var emailDto = GetEmailDTO();
            var expectedErrorMessage = "RecaptchaRequestFailed";

            this.SetupMockHttpMessageHandlerReturnsFail();
            this.SetupMockEmailServiceReturnsOK();
            this.SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(this.mockEmailService.Object, this.mockLogger.Object, this.mockStringLocalizer.Object, this.mockHttpClientFactory.Object, this.mockConfiguration.Object, this.mockMessageDataAbstractFactory.Object);

            // act
            var result = await handler.Handle(new SendEmailCommand(emailDto), CancellationToken.None);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_WhenTokenIsIncorrect()
        {
            // arrange
            var reCaptchaResponseDto = GetReCaptchaResponseDTO(false);
            var emailDto = GetEmailDTO();
            var expectedErrorMessage = "InvalidCaptcha";

            this.SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            this.SetupMockEmailServiceReturnsOK();
            this.SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(this.mockEmailService.Object, this.mockLogger.Object, this.mockStringLocalizer.Object, this.mockHttpClientFactory.Object, this.mockConfiguration.Object, this.mockMessageDataAbstractFactory.Object);

            // act
            var result = await handler.Handle(new SendEmailCommand(emailDto), CancellationToken.None);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        [Fact]
        public async Task ShouldReturnFail_WhenEmailWasNotSent()
        {
            // arrange
            var reCaptchaResponseDto = GetReCaptchaResponseDTO(true);
            var emailDto = GetEmailDTO();
            var expectedErrorMessage = "FailedToSendEmailMessage";

            this.SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            this.SetupMockEmailServiceReturnsFalse();
            this.SetupMockStringLocalizer(expectedErrorMessage);
            this.SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(this.mockEmailService.Object, this.mockLogger.Object, this.mockStringLocalizer.Object, this.mockHttpClientFactory.Object, this.mockConfiguration.Object, this.mockMessageDataAbstractFactory.Object);

            // act
            var result = await handler.Handle(new SendEmailCommand(emailDto), CancellationToken.None);

            // assert
            Assert.False(result.IsSuccess);
            Assert.Equal(expectedErrorMessage, result.Errors[0].Message);
        }

        private static ReCaptchaResponseDto GetReCaptchaResponseDTO(bool success)
        {
            return new ReCaptchaResponseDto()
            {
                Success = success,
            };
        }

        private static EmailDto GetEmailDTO()
        {
            return new EmailDto()
            {
                Content = "Test Content",
                From = "testemail@gmail.com",
                Token = "Token",
            };
        }

        private void SetupMockHttpMessageHandlerReturnsOK(ReCaptchaResponseDto reCaptchaResponseDto)
        {
            this.mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = JsonContent.Create<ReCaptchaResponseDto>(reCaptchaResponseDto) });
        }

        private void SetupMockHttpMessageHandlerReturnsFail()
        {
            this.mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });
        }

        private void SetupMockEmailServiceReturnsOK()
        {
            this.mockEmailService
                .Setup<Task<bool>>(service => service.SendEmailAsync(It.IsAny<MessageData>()))
                .Returns(Task.FromResult(true));
        }

        private void SetupMockEmailServiceReturnsFalse()
        {
            this.mockEmailService
                .Setup<Task<bool>>(service => service.SendEmailAsync(It.IsAny<MessageData>()))
                .Returns(Task.FromResult(false));
        }

        private void SetupMockStringLocalizer(string key)
        {
            var localizedString = new LocalizedString(key, key);
            this.mockStringLocalizer.Setup(_ => _[key]).Returns(localizedString);
        }

        private void SetupMockIHttpClientFactory()
        {
            var client = new HttpClient(this.mockHttpMessageHandler.Object);
            this.mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        }
    }
}
