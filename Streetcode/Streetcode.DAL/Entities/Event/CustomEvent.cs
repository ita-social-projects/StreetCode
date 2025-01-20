using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.DAL.Entities.Event
{
    public class CustomEvent : Event
    {
        [MaxLength(200)]
        public string? Location { get; set; }
        [MaxLength(100)]
        public string? Organizer { get; set; }
        [MaxLength(100)]
        public string? DateString { get; set; }
    }
}
