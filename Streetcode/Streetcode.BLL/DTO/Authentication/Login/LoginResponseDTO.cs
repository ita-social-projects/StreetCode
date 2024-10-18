using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.DTO.Authentication.Login
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
