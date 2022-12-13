using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates;

public class CoordinateProfile: Profile
{
    public CoordinateProfile()
    {
        CreateMap<Coordinate, CoordinatesDTO>().ReverseMap();
    }
}