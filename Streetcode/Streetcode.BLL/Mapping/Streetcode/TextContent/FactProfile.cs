using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.BLL.Mapping.Streetcode.TextContent;

public class FactProfile : Profile
{
    public FactProfile()
    {
        CreateMap<Fact, FactDto>().ReverseMap();
        CreateMap<Fact, FactUpdateCreateDto>().ReverseMap();
        CreateMap<FactUpdateDto, Fact>()
          .ForMember(x => x.Streetcode, opt => opt.MapFrom(src => null as StreetcodeContent))
          .ForMember(x => x.Image, opt => opt.MapFrom(src => src.ModelState == Enums.ModelState.Deleted ? new Image { Id = src.ImageId, BlobName = string.Empty, MimeType = string.Empty } : null))
          .ReverseMap();
    }
}
