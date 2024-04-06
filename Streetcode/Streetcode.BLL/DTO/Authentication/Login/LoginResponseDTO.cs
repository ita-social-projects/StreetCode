using Streetcode.BLL.DTO.Users;

namespace Streetcode.BLL.DTO.Authentication.Login
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }
    }
}
