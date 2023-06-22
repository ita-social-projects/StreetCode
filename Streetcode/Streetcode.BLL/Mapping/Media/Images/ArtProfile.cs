using AutoMapper;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ArtProfile : Profile
{
    public ArtProfile()
    {
        CreateMap<Art, ArtDTO>().ReverseMap();
        CreateMap<ArtCreateUpdateDTO, Art>()
        .ForMember(x => x.Image, opt => opt.Ignore())
        .ForMember(x => x.ImageId, opt => opt.MapFrom(src => src.Image.Id))
      .ReverseMap();
    }
}
