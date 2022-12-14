using AutoMapper;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Toponyms;


public class ToponymProfile : Profile
{
    public ToponymProfile()
    {
        CreateMap<Toponym, ToponymDTO>().ReverseMap();
    }
}