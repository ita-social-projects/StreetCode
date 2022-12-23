using AutoMapper;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.BLL.Mapping.Streetcode;

public class RelatedFigureProfile : Profile
{
    public RelatedFigureProfile()
    {
        CreateMap<RelatedFigure, RelatedFigureDTO>().ReverseMap();
    }
}