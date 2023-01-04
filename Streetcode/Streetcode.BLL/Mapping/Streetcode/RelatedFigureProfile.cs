using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode;

public class RelatedFigureProfile : Profile
{
    public RelatedFigureProfile()
    {
        CreateMap<EventStreetcode, RelatedFigureDTO>()
            .ForPath(dto => dto.Title, conf => conf.MapFrom(e => e.Title))
            .ForPath(dto => dto.Image, conf => conf.MapFrom(e => e.Images.FirstOrDefault()));

        CreateMap<PersonStreetcode, RelatedFigureDTO>()
            .ForPath(dto => dto.Title, conf => conf
                .MapFrom(e => (e.Rank == null) ? e.FirstName + " " + e.LastName : e.Rank + " " + e.FirstName + " " + e.LastName))
            .ForPath(dto => dto.Image, conf => conf.MapFrom(e => e.Images.FirstOrDefault()));
    }
}