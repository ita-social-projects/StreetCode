using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Team
{
    public class GetAllPositionsDto
    {
        public int TotalAmount { get; set; }
        public IEnumerable<PositionDto> Positions { get; set; } = new List<PositionDto>();
    }
}
