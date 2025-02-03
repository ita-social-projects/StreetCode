using AutoMapper;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.Types;

namespace Streetcode.BLL.Mapping.Streetcode;

public class RelatedFigureProfile : Profile
{
    public RelatedFigureProfile()
    {
        CreateMap<EventStreetcode, RelatedFigureDto>()
            .ForPath(dto => dto.Title, conf => conf
                .MapFrom(e => e.Title))
            .ForPath(dto => dto.Url, conf => conf
                .MapFrom(e => e.TransliterationUrl))
            .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).LastOrDefault()));

        CreateMap<PersonStreetcode, RelatedFigureDto>()
            .ForPath(dto => dto.Url, conf => conf
                .MapFrom(e => e.TransliterationUrl))
            .ForPath(dto => dto.ImageId, conf => conf
                .MapFrom(e => e.Images.Select(i => i.Id).LastOrDefault()));

        CreateMap<RelatedFigureUpdateDto, RelatedFigure>();
        CreateMap<StreetcodeContent, RelatedFigureShortDto>();
    }
}
