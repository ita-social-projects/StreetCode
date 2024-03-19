using Streetcode.WebApi.Utils.Settings;

namespace Streetcode.WebApi.Utils
{
    public static class SettingsExtracter
    {
        public static CorsSettings GetCorsSettings(IConfiguration configuration)
        {
            return new CorsSettings()
            {
                AllowedHeaders = GetAllowedCorsValues(configuration, "AllowedHeaders"),
                AllowedMethods = GetAllowedCorsValues(configuration, "AllowedMethods"),
                AllowedOrigins = GetAllowedCorsValues(configuration, "AllowedOrigins"),
                ExposedHeaders = GetAllowedCorsValues(configuration, "ExposedHeaders"),
                PreflightMaxAge = int.Parse(configuration.GetValue<string>("CORS:PreflightMaxAge") ?? "600"),
            };
        }

        private static string[] GetAllowedCorsValues(IConfiguration configuration, string key)
        {
            string? allowedCorsValuesStringified = configuration.GetValue<string>($"CORS:{key}");
            return (allowedCorsValuesStringified ?? "*")
                    .Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
