using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.AdditionalContent.Coordinates.Update
{
    public class StreetcodeCoordinateUpdateDTO : StreetcodeCoordinateDTO, IModelState
    {
        public ModelState ModelState { get; set; }
    }
}
