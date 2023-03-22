using AutoMapper;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class AudioProfile : Profile
{
    public AudioProfile()
    {
        CreateMap<Audio, AudioDTO>();

        CreateMap<AudioFileBaseCreateDTO, Audio>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType))
            .ForMember(dest => dest.StreetcodeId, opt => opt.MapFrom(src => src.StreetcodeId))
            .ForMember(dest => dest.BlobStorageName, opt => opt.MapFrom(src => src.Name));
    }
}