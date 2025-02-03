using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Timeline
{
    public class GetAllHistoricalContextDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<HistoricalContextDto> HistoricalContexts { get; set; } = new List<HistoricalContextDto>();
    }
}
