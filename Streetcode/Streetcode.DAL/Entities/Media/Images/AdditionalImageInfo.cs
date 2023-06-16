using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.DAL.Entities.Media.Images
{
    [Table("additional_image_info", Schema = "media")]
    public class AdditionalImageInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(200)]
        public string? Alt { get; set; }

        public Image? Image { get; set; }
    }
}
