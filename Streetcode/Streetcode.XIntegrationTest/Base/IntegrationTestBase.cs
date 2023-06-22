using Microsoft.Extensions.Configuration;
using Streetcode.WebApi.Extensions;

namespace Streetcode.XIntegrationTest
{
    public class IntegrationTestBase
    {
        protected IConfigurationRoot Configuration { get; }

        public IntegrationTestBase()
        {
            Environment.SetEnvironmentVariable("STREETCODE_ENVIRONMENT", "IntegrationTests");
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "Local";

            IConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .CustomConfigure(environment);

            Configuration = configBuilder.Build();
        }
    }
}