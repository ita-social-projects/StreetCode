using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Team;

public abstract class TeamMemberCreateUpdateDTO
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool? IsMain { get; set; }
    public int? ImageId { get; set; }
    public List<PositionDTO>? Positions { get; set; } = new List<PositionDTO>();
}