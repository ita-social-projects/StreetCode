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
            ConfigurationBuilder configBuilder = new ConfigurationBuilder()
                .CustomConfigure();

            Configuration = configBuilder.Build();
        }
    }
}