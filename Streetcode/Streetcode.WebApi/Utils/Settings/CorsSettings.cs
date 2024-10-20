namespace Streetcode.WebApi.Utils.Settings
{
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; } = null!;
        public string[] AllowedHeaders { get; set; } = null!;
        public string[] AllowedMethods { get; set; } = null!;
        public string[] ExposedHeaders { get; set; } = null!;
        public int PreflightMaxAge { get; set; }
    }
}
