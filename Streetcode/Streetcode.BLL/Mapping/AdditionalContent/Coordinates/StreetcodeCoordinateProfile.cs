using AutoMapper;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates;

namespace Streetcode.BLL.Mapping.AdditionalContent.Coordinates;

public class StreetcodeCoordinateProfile : Profile
{
   public StreetcodeCoordinateProfile()
   {
        CreateMap<StreetcodeCoordinate, StreetcodeCoordinateDTO>().ReverseMap();
   }
}