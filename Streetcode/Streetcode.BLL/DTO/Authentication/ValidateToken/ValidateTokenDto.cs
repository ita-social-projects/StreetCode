namespace Streetcode.BLL.DTO.Authentication.ValidateToken
{
    public class ValidateTokenDto
    {
        public string Token { get; set; } = string.Empty;

        public string Purpose { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string TokenProvider { get; set; } = "Default";
    }
}