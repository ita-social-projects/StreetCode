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
            .ForPath(dto => dto.ImageId, conf => conf.MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));

        CreateMap<PersonStreetcode, RelatedFigureDTO>()
            .ForPath(dto => dto.Title, conf => conf
                .MapFrom(e => (e.Rank == null) ? $"{e.FirstName} {e.LastName}" : $"{e.FirstName} {e.LastName}"))
            .ForPath(dto => dto.ImageId, conf => conf.MapFrom(e => e.Images.Select(i => i.Id).FirstOrDefault()));
    }
}