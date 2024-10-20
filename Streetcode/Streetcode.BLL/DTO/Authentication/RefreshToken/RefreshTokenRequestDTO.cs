namespace Streetcode.BLL.DTO.Authentication.RefreshToken
{
    public class RefreshTokenRequestDTO
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
