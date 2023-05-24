using AutoMapper;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ArtProfile : Profile
{
    public ArtProfile()
    {
        CreateMap<Art, ArtDTO>().ReverseMap();
        CreateMap<ArtCreateDTO, StreetcodeArt>()
            .ForMember(x => x.Index, src => src.MapFrom(x => x.Index))
            .ForPath(x => x.Art.ImageId, src => src.MapFrom(x => x.ImageId))
            .ForPath(x => x.Art.Description, src => src.MapFrom(x => x.Description))
            .ForPath(x => x.Art.Image.MimeType, src => src.MapFrom(x => x.MimeType))
            .ForPath(x => x.Art.Image.Title, src => src.MapFrom(x => x.Title))
            .ReverseMap();
    }
}
