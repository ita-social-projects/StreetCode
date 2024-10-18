using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.BLL.DTO.Team
{
    public class GetAllTeamDTO
    {
        public int TotalAmount { get; set; }
        public IEnumerable<TeamMemberDTO> TeamMembers { get; set; } = new List<TeamMemberDTO>();
    }
}
