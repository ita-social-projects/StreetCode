using AutoMapper;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Toponyms;

public class ToponymProfile : Profile
{
    public ToponymProfile()
    {
        CreateMap<Toponym, ToponymDTO>().ReverseMap();
        CreateMap<Toponym, StreetcodeToponymUpdateDTO>()
			.ForMember(tu => tu.ToponymId, conf => conf.MapFrom(t => t.Id));
	}
}