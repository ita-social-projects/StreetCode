using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Update;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates;

public class StreetcodeCoordinateProfile : Profile
{
   public StreetcodeCoordinateProfile()
   {
        CreateMap<StreetcodeCoordinate, StreetcodeCoordinateDTO>().ReverseMap();
        CreateMap<StreetcodeCoordinate, StreetcodeCoordinateUpdateDTO>().ReverseMap();
   }
}