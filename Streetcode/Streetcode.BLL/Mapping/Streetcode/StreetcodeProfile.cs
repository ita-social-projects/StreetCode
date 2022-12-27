using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Streetcode;

public class StreetcodeProfile : Profile
{
	public StreetcodeProfile()
	{
        CreateMap<StreetcodeContent, StreetcodeDTO>().ReverseMap();
    }
}