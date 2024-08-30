using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Team;

public abstract class TeamMemberLinkCreateUpdateDTO
{
    public LogoType? LogoType { get; set; }
    public string TargetUrl { get; set; }
    public int TeamMemberId { get; set; }
}