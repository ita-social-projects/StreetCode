namespace Streetcode.BLL.DTO.Users
{
    public class UserDTO : BaseUserDTO
    {
        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
