using AutoMapper;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.DAL.Entities.Sources;

namespace Streetcode.BLL.Mapping.Sources
{
    internal class StreetcodeCategoryContentProfile : Profile
    {
        public StreetcodeCategoryContentProfile()
        {
            CreateMap<StreetcodeCategoryContent, StreetcodeCategoryContentDTO>()
                .ReverseMap();

            CreateMap<StreetcodeCategoryContentUpdateDTO, StreetcodeCategoryContent>()
              .ForMember(x => x.Streetcode, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
