using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Analytics
{
    [Table("qr_coordinates", Schema = "coordinates")]
    public class StatisticRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QrId { get; set; }
        public int Count { get; set; }

        [MaxLength(150)]
        public string Address { get; set; }

        public int StreetcodeId { get; set; }
        public StreetcodeContent? Streetcode { get; set; }

        public int StreetcodeCoordinateId { get; set; }
        public StreetcodeCoordinate StreetcodeCoordinate { get; set; }
     }
}
