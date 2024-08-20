using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Team;

public abstract class TeamMemberCreateUpdateDTO<TLinkDTO>
    where TLinkDTO : TeamMemberLinkCreateUpdateDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? IsMain { get; set; }
    public int? ImageId { get; set; }

    public List<TLinkDTO>? TeamMemberLinks { get; set; }
    public List<PositionDTO>? Positions { get; set; }
}