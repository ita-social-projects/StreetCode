using AutoMapper;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Mapping.Authentication
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, RegisterResponseDTO>().ReverseMap();
            CreateMap<User, RegisterRequestDTO>().ReverseMap();
        }
    }
}
