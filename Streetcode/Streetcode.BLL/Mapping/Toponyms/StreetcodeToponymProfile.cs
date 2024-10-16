using AutoMapper;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.BLL.Mapping.Toponyms
{
  public class StreetcodeToponymProfile : Profile
	{
		public StreetcodeToponymProfile()
		{
			CreateMap<StreetcodeToponym, StreetcodeToponymCreateUpdateDTO>().ReverseMap();
		}
	}
}
