using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Update;
using Streetcode.BLL.DTO.Streetcode.Update.Interfaces;
using Streetcode.BLL.Enums;

namespace Streetcode.BLL.DTO.Analytics.Update
{
    public class StatisticRecordUpdateDTO : IModelState
    {
        public int Id { get; set; }
        public int QrId { get; set; }
        public int Count { get; set; }
        public string Address { get; set; }
        public int StreetcodeId { get; set; }
        public StreetcodeCoordinateUpdateDTO StreetcodeCoordinate { get; set; }
        public ModelState ModelState { get; set; }
    }
}
