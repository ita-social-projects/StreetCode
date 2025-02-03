using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.DTO.Analytics
{
    public class StatisticRecordDto
    {
        public StreetcodeCoordinateDto StreetcodeCoordinate { get; set; } = null!;
        public int QrId { get; set; }
        public int Count { get; set; }
        public string Address { get; set; } = null!;
    }
}
