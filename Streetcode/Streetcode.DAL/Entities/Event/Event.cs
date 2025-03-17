using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Streetcode.DAL.Enums;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.DAL.Entities.Event
{
    [Table("event", Schema = "events")]
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Title { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public string EventType { get; set; }
        public List<EventStreetcodes> EventStreetcodes { get; set; } = new();
    }
}
