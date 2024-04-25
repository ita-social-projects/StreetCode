using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.DTO.Analytics
{
    public class StatisticRecordDTO
    {
        public StreetcodeCoordinateDTO StreetcodeCoordinate { get; set; }
        public int QrId { get; set; }
        public int Count { get; set; }
        public string Address { get; set; }
    }
}
