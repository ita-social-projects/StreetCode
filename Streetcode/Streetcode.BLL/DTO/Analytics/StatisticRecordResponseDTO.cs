using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
namespace Streetcode.BLL.DTO.Analytics
{
    public class StatisticRecordResponseDTO
    {
        public int Id { get; set; }
        public StreetcodeCoordinateDTO StreetcodeCoordinate { get; set; } = null!;
        public int QrId { get; set; }
        public int Count { get; set; }
        public string Address { get; set; } = null!;
    }
}
