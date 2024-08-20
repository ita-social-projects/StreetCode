using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.DTO.Team;

public abstract class TeamMemberLinkCreateUpdateDTO
{
    public LogoTypeDTO LogoType { get; set; }
    public string TargetUrl { get; set; }
    public int TeamMemberId { get; set; }
}