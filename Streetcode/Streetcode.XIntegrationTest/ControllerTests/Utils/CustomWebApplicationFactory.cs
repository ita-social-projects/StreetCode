using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Streetcode.BLL.Interfaces.Email;
using Streetcode.BLL.Models.Email.Messages.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
        where TProgram : class
    {
        public Mock<IEmailService> EmailServiceMock { get; private set; } = new Mock<IEmailService>();

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

                EmailServiceMock.Setup(es => es.SendEmailAsync(It.IsAny<MessageData>()))
                    .ReturnsAsync(true);

                services.AddSingleton(EmailServiceMock.Object);
            });
        }
    }
}
