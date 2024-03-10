using Streetcode.WebApi.Utils.Settings;

namespace Streetcode.WebApi.Utils
{
    public static class SettingsExtracter
    {
        public static CorsSettings GetCorsSettings(IConfiguration configuration)
        {
            return new CorsSettings()
            {
                AllowedHeaders = GetArrayFromString(configuration.GetValue<string>("CORS:AllowedHeaders")),
                AllowedMethods = GetArrayFromString(configuration.GetValue<string>("CORS:AllowedMethods")),
                AllowedOrigins = GetArrayFromString(configuration.GetValue<string>("CORS:AllowedOrigins")),
                PreflightMaxAge = int.Parse(configuration.GetValue<string>("CORS:PreflightMaxAge") ?? "600"),
            };
        }

        private static string[] GetArrayFromString(string? value)
        {
            return (value ?? "*").Split(',', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
