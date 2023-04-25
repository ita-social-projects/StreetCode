using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.BLL.DTO.Analytics
{
    public class StatisticRecordDTO
    {
        public int Id { get; set; }
        public StreetcodeCoordinate StreetcodeCoordinate { get; set; }
        public int CoordinateId { get; set; }
        public int QrId { get; set; }
        public string Title { get; set; }
        public int Count { get; set; }
        public string Address { get; set; }
    }
}
