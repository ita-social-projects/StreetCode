using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Team.Abstractions;

public abstract class TeamMemberLinkCreateUpdateDTO
{
    public LogoType? LogoType { get; set; }
    public string TargetUrl { get; set; } = null!;
    public int TeamMemberId { get; set; }
}