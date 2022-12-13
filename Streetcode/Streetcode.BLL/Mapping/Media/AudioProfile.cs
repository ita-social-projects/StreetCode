using AutoMapper;
using Streetcode.BLL.DTO.Media;
using Streetcode.DAL.Entities.Media;

namespace Streetcode.BLL.Mapping.Media;


public class AudioProfile: Profile
{
    public AudioProfile()
    {
        CreateMap<Audio, AudioDTO>().ReverseMap();
    }
}