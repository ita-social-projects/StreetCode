using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates;

public class ToponymCoordinateProfile : Profile
{
    public ToponymCoordinateProfile()
    {
        CreateMap<ToponymCoordinate, ToponymCoordinateDTO>().ReverseMap();
    }
}