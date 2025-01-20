using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Event.CreateUpdate
{
    public class CreateUpdateCustomEventDTO : CreateUpdateEventDTO
    {
        public string? Location { get; set; }
        public string? Organizer { get; set; }
    }
}
