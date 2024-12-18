namespace Streetcode.BLL.Utils.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public string Key { get; set; } = null!;
        public double AccessTokenLifetimeInMinutes { get; set; }
        public double RefreshTokenLifetimeInDays { get; set; }
    }
}
