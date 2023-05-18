using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public int ImageId { get; set; }
        public List<TeamMemberLinkDTO> TeamMemberLinks { get; set; } = new List<TeamMemberLinkDTO>();
        public List<PositionDTO> Positions { get; set; } = new List<PositionDTO>();
    }
}
