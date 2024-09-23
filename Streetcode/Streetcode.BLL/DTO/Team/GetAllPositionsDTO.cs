using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Team
{
    public class GetAllPositionsDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<PositionDTO> Positions { get; set; } = new List<PositionDTO>();
    }
}
