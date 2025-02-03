using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.DTO.Authentication.Login
{
    public class LoginResponseDto
    {
        public UserDto User { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
