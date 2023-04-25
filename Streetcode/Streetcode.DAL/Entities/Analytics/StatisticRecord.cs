using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;

namespace Streetcode.DAL.Entities.Analytics
{
    [Table("qr_statistics", Schema = "analytics")]
    public class StatisticRecord
    {
        public int Id { get; set; }
        [ForeignKey("CoordinateId")]
        public StreetcodeCoordinate StreetcodeCoordinate { get; set; }
        public int CoordinateId { get; set; }
        public int QrId { get; set; }
        [MaxLength(150)]
        public string Title { get; set; }
        public int Count { get; set; }
        [MaxLength(150)]
        public string Address { get; set; }
    }
}
