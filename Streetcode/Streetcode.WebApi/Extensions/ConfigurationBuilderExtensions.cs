namespace Streetcode.WebApi.Extensions
{
    public static class ConfigurationBuilderExtensions
    {
        public static ConfigurationBuilder CustomConfigure(this ConfigurationBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("STREETCODE_ENVIRONMENT") ?? "Local";
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("STREETCODE_");

            return builder;
        }
    }
}