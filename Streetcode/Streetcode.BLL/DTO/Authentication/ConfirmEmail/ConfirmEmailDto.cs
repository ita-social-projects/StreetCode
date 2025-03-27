namespace Streetcode.BLL.DTO.Authentication.ConfirmEmail
{
    public class ConfirmEmailDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}