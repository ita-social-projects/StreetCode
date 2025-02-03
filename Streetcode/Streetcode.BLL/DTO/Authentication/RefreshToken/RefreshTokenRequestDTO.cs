namespace Streetcode.BLL.DTO.Authentication.RefreshToken
{
    public class RefreshTokenRequestDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
