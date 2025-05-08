using System.Net;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Streetcode.BLL.Services.Authentication;
using Xunit;

namespace Streetcode.XUnitTest.Services.Authentication.CaptchaServiceTest
{
    public class ValidateReCaptchaAsyncTest
    {
        private const string ReCaptchaUrl = "https://fakeUrl.com";
        private const string ReCaptchaSecretKey = "fake_secret_key";
        private const string TestReCaptchaToken = "test_reCaptcha_token";
        private readonly IConfiguration _fakeConfiguration;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateReCaptchaAsyncTest"/> class.
        /// </summary>
        public ValidateReCaptchaAsyncTest()
        {
            _fakeConfiguration = GetFakeConfiguration();
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task ShouldReturnSuccess_ValidCaptchaToken()
        {
            // Arrange.
            SetupMockHttpClient(true);
            var captchaService = GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(TestReCaptchaToken);

            // Assert.
            Assert.True(captchaValidationResult.IsSuccess);
        }

        [Fact]
        public async Task ShouldFail_ReCaptchaRequestFailed()
        {
            // Arrange.
            SetupMockHttpClient(false);
            var captchaService = GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(TestReCaptchaToken);

            // Assert.
            Assert.True(captchaValidationResult.IsFailed);
        }

        [Fact]
        public async Task ShouldFail_InvalidCaptcha()
        {
            // Arrange.
            SetupMockHttpClient(true, false);
            var captchaService = GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(TestReCaptchaToken);

            // Assert.
            Assert.True(captchaValidationResult.IsFailed);
        }

        private void SetupMockHttpClient(bool isRequestSuccess, bool isCaptchaValid = true)
        {
            HttpStatusCode returnStatusCode = isRequestSuccess ? HttpStatusCode.OK : HttpStatusCode.BadRequest;
            var httpResponseMessage = new HttpResponseMessage(returnStatusCode)
            {
                Content = new StringContent($$"""{ "success": {{isCaptchaValid.ToString().ToLower()}}, "errorCodes": [] }"""),
            };
            _mockHttpMessageHandler
                .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(httpResponseMessage);
        }

        private IConfiguration GetFakeConfiguration()
        {
            var appSettingsStub = new Dictionary<string, string?>
            {
                { "ReCaptcha:Url", ReCaptchaUrl },
                { "ReCaptcha:SecretKey", ReCaptchaSecretKey },
            };
            var fakeConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

            return fakeConfiguration;
        }

        private CaptchaService GetCaptchaService()
        {
            return new CaptchaService(
                _fakeConfiguration,
                new HttpClient(_mockHttpMessageHandler.Object));
        }
    }
}
