using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberLinkDTO
    {
        public int Id { get; set; }
        public LogoTypeDTO LogoType { get; set; }
        public string TargetUrl { get; set; } = null!;
        public int TeamMemberId { get; set; }
    }
}
