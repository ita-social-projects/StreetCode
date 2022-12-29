using AutoMapper;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.BLL.Mapping.Media.Images;

public class ArtProfile : Profile
{
    public ArtProfile()
    {
        CreateMap<Art, ArtDTO>().ForMember(x => x.Streetcodes, conf => conf.Ignore());
    }
}
