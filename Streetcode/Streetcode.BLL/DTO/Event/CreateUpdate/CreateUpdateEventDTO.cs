using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Event.CreateUpdate
{
    public class CreateUpdateEventDTO
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
