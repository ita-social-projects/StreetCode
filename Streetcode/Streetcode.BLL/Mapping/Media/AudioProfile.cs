using AutoMapper;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;

public class AudioProfile : Profile
{
    public AudioProfile()
    {
        CreateMap<Audio, AudioDto>()
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<AudioFileBaseCreateDto, Audio>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<AudioFileBaseUpdateDto, Audio>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.MimeType, opt => opt.MapFrom(src => src.MimeType));

        CreateMap<AudioUpdateDto, Audio>()
            .ForMember(a => a.BlobName, opt => opt.MapFrom(x => string.Empty))
            .ForMember(a => a.MimeType, opt => opt.MapFrom(x => string.Empty));
	}
}
