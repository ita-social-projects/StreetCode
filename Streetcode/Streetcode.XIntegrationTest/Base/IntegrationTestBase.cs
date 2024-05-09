using Microsoft.Extensions.Configuration;
using Streetcode.WebApi.Extensions;

namespace Streetcode.XIntegrationTest
{
    public class IntegrationTestBase
    {
        protected IConfigurationRoot Configuration { get; }

        public IntegrationTestBase()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "IntegrationTests");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Local";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .ConfigureCustom(environment);

            this.Configuration = configBuilder.Build();
        }
    }
}
