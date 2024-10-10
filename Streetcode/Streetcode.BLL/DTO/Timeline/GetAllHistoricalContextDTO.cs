using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Timeline
{
    public class GetAllHistoricalContextDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<HistoricalContextDTO> HistoricalContexts { get; set; } = new List<HistoricalContextDTO>();
    }
}
