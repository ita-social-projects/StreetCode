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
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILoggerService> _mockLogger;
        private readonly Mock<IStringLocalizer<SendEmailHandler>> _mockStringLocalizer;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IMessageDataAbstractFactory> _mockMessageDataAbstractFactory;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;

        public SendEmailHandlerTests()
        {
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILoggerService>();
            _mockStringLocalizer = new Mock<IStringLocalizer<SendEmailHandler>>();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockConfiguration = new Mock<IConfiguration>();
            _mockMessageDataAbstractFactory = new Mock<IMessageDataAbstractFactory>();
            _mockStringLocalizer.Setup(x => x["RecaptchaRequestFailed"]).Returns(new LocalizedString("RecaptchaRequestFailed", "RecaptchaRequestFailed"));
            _mockStringLocalizer.Setup(x => x["InvalidCaptcha"]).Returns(new LocalizedString("InvalidCaptcha", "InvalidCaptcha"));
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task ShouldReturnSuccessfully_EmailIsCorrect()
        {
            // arrange
            var reCaptchaResponseDto = GetReCaptchaResponseDTO(true);
            var emailDto = GetEmailDTO();

            SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            SetupMockEmailServiceReturnsOK();
            SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(_mockEmailService.Object, _mockLogger.Object, _mockStringLocalizer.Object, _mockHttpClientFactory.Object, _mockConfiguration.Object, _mockMessageDataAbstractFactory.Object);

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

            SetupMockHttpMessageHandlerReturnsFail();
            SetupMockEmailServiceReturnsOK();
            SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(_mockEmailService.Object, _mockLogger.Object, _mockStringLocalizer.Object, _mockHttpClientFactory.Object, _mockConfiguration.Object, _mockMessageDataAbstractFactory.Object);

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

            SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            SetupMockEmailServiceReturnsOK();
            SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(_mockEmailService.Object, _mockLogger.Object, _mockStringLocalizer.Object, _mockHttpClientFactory.Object, _mockConfiguration.Object, _mockMessageDataAbstractFactory.Object);

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

            SetupMockHttpMessageHandlerReturnsOK(reCaptchaResponseDto);
            SetupMockEmailServiceReturnsFalse();
            SetupMockStringLocalizer(expectedErrorMessage);
            SetupMockIHttpClientFactory();

            var handler = new SendEmailHandler(_mockEmailService.Object, _mockLogger.Object, _mockStringLocalizer.Object, _mockHttpClientFactory.Object, _mockConfiguration.Object, _mockMessageDataAbstractFactory.Object);

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

        private static EmailDTO GetEmailDTO()
        {
            return new EmailDTO()
            {
                Content = "Test Content",
                From = "testemail@gmail.com",
                Token = "Token",
            };
        }

        private void SetupMockHttpMessageHandlerReturnsOK(ReCaptchaResponseDto reCaptchaResponseDto)
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = JsonContent.Create<ReCaptchaResponseDto>(reCaptchaResponseDto) });
        }

        private void SetupMockHttpMessageHandlerReturnsFail()
        {
            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest });
        }

        private void SetupMockEmailServiceReturnsOK()
        {
            _mockEmailService
                .Setup<Task<bool>>(service => service.SendEmailAsync(It.IsAny<MessageData>()))
                .Returns(Task.FromResult(true));
        }

        private void SetupMockEmailServiceReturnsFalse()
        {
            _mockEmailService
                .Setup<Task<bool>>(service => service.SendEmailAsync(It.IsAny<MessageData>()))
                .Returns(Task.FromResult(false));
        }

        private void SetupMockStringLocalizer(string key)
        {
            var localizedString = new LocalizedString(key, key);
            _mockStringLocalizer.Setup(_ => _[key]).Returns(localizedString);
        }

        private void SetupMockIHttpClientFactory()
        {
            var client = new HttpClient(_mockHttpMessageHandler.Object);
            _mockHttpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
        }
    }
}
