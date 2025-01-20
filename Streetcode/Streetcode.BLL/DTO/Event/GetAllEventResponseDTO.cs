using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Event
{
    public class GetAllEventResponseDTO : EventDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<EventDTO> Jobs { get; set; } = new List<EventDTO>();
    }
}
