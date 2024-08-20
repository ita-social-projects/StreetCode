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
        private readonly string _reCaptchaUrl = "https://fakeUrl.com";
        private readonly string _reCaptchaSecretKey = "fake_secret_key";
        private readonly string _testReCaptchaToken = "test_reCaptcha_token";
        private readonly IConfiguration _fakeConfiguration;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidateReCaptchaAsyncTest"/> class.
        /// </summary>
        public ValidateReCaptchaAsyncTest()
        {
            this._fakeConfiguration = this.GetFakeConfiguration();
            this._mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        }

        [Fact]
        public async Task ShouldReturnSuccess_ValidCaptchaToken()
        {
            // Arrange.
            this.SetupMockHttpClient(true);
            var captchaService = this.GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(this._testReCaptchaToken);

            // Assert.
            Assert.True(captchaValidationResult.IsSuccess);
        }

        [Fact]
        public async Task ShouldFail_ReCaptchaRequestFailed()
        {
            // Arrange.
            this.SetupMockHttpClient(false);
            var captchaService = this.GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(this._testReCaptchaToken);

            // Assert.
            Assert.True(captchaValidationResult.IsFailed);
        }

        [Fact]
        public async Task ShouldFail_InvalidCaptcha()
        {
            // Arrange.
            this.SetupMockHttpClient(true, false);
            var captchaService = this.GetCaptchaService();

            // Act.
            var captchaValidationResult = await captchaService.ValidateReCaptchaAsync(this._testReCaptchaToken);

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
            this._mockHttpMessageHandler
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
                { "ReCaptcha:Url", this._reCaptchaUrl },
                { "ReCaptcha:SecretKey", this._reCaptchaSecretKey },
            };
            var fakeConfiguration = new ConfigurationBuilder()
            .AddInMemoryCollection(appSettingsStub)
            .Build();

            return fakeConfiguration;
        }

        private CaptchaService GetCaptchaService()
        {
            return new CaptchaService(
                this._fakeConfiguration,
                new HttpClient(this._mockHttpMessageHandler.Object));
        }
    }
}
