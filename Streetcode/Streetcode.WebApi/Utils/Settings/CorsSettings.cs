namespace Streetcode.WebApi.Utils.Settings
{
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; }
        public string[] AllowedHeaders { get; set; }
        public string[] AllowedMethods { get; set; }
        public int PreflightMaxAge { get; set; }
    }
}
