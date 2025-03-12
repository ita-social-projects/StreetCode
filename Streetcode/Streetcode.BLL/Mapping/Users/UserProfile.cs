using AutoMapper;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Mapping.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();

            CreateMap<User, UserProfileDTO>().ReverseMap();

            CreateMap<User, UpdateUserDTO>().ReverseMap();
        }
    }
}
