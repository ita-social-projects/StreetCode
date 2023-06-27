using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.DAL.Entities.Team
{
    [Table("team_member_positions", Schema = "team")]
    public class TeamMemberPositions
    {
        public int TeamMemberId { get; set; }
        public Positions Positions { get; set; }
        public TeamMember TeamMember { get; set; }
        public int PositionsId { get; set; }
    }
}
