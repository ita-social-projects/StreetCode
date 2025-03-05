using Google.Apis.Auth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Streetcode.BLL.Interfaces.Authentication;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Models.Email.Messages.Base;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        public Mock<IEmailService> EmailServiceMock { get; private set; } = new Mock<IEmailService>();

        public Mock<IGoogleService> GoogleServiceMock { get; private set; } = new Mock<IGoogleService>();

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

                services.AddSingleton(EmailServiceMock.Object);
                services.AddSingleton(GoogleServiceMock.Object);
            });
        }
    }
}
