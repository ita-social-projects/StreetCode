using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Event
{
    public class CustomEventDto : EventDto
    {
        public string DateString { get; set; }
        public string? Location { get; set; }
        public string? Organizer { get; set; }
    }
}
