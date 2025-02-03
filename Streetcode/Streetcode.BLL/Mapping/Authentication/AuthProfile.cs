using AutoMapper;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Mapping.Authentication
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<User, RegisterResponseDto>().ReverseMap();
            CreateMap<User, RegisterRequestDto>().ReverseMap();
        }
    }
}
