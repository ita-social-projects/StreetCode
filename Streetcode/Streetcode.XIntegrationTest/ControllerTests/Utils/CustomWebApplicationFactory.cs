using Google.Apis.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Models.Email.Messages.Base;
using Streetcode.DAL.Entities.Users;
using System.Net;
using FluentResults;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        public Mock<IEmailService> EmailServiceMock { get; private set; } = new Mock<IEmailService>();

        public Mock<IGoogleService> GoogleServiceMock { get; private set; } = new Mock<IGoogleService>();

        public Mock<ICaptchaService> CaptchaServiceMock { get; private set; } = new Mock<ICaptchaService>();

        public Mock<IHttpClientFactory> HttpClientFactoryMock { get; private set; } = new Mock<IHttpClientFactory>();


        public CustomWebApplicationFactory()
        {
            SetupMockHttpClient();
            SetupMockCaptchaService();
        }

        public void SetupMockGoogleLogin(User user, string? token = null)
        {
            if (token is null)
            {
                GoogleServiceMock
                    .Setup(gs => gs.ValidateGoogleToken("invalid_google_id_token"))
                    .ThrowsAsync(new InvalidJwtException("Invalid Google Token"));
            }
            else
            {
                GoogleServiceMock
                    .Setup(gs => gs.ValidateGoogleToken(It.IsAny<string>()))
                    .ReturnsAsync(new GoogleJsonWebSignature.Payload
                    {
                        Email = user.Email,
                        GivenName = user.Name,
                        FamilyName = user.Surname,
                        Subject = "google-subject-id",
                    });
            }
        }

        public void SetupMockEmailService(bool success = true)
        {
            EmailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<MessageData>()))
                .ReturnsAsync(success);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests");

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEmailService));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                var googleDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IGoogleService));
                if (googleDescriptor != null)
                {
                    services.Remove(googleDescriptor);
                }

                var httpClientFactoryDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IHttpClientFactory));
                if (httpClientFactoryDescriptor != null)
                {
                    services.Remove(httpClientFactoryDescriptor);
                }

                var captchaDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ICaptchaService));
                if (captchaDescriptor != null)
                {
                    services.Remove(captchaDescriptor);
                }

                services.AddSingleton(EmailServiceMock.Object);
                services.AddSingleton(GoogleServiceMock.Object);
                services.AddSingleton(HttpClientFactoryMock.Object);
                services.AddSingleton(CaptchaServiceMock.Object);
            });
        }

        private void SetupMockHttpClient()
        {
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"success\": true}"),
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            HttpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }

        private void SetupMockCaptchaService()
        {
            this.CaptchaServiceMock
                .Setup(service => service.ValidateReCaptchaAsync(It.IsAny<string>(), It.IsAny<CancellationToken?>()))
                .ReturnsAsync(Result.Ok());
        }
    }
}
