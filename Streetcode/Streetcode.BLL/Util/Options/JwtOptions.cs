﻿namespace Streetcode.BLL.Utils.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public double AccessTokenLifetimeInMinutes { get; set; }
        public double RefreshTokenLifetimeInDays { get; set; }
    }
}
