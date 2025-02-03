namespace Streetcode.BLL.DTO.Team.Abstractions;

public abstract class TeamMemberCreateUpdateDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? IsMain { get; set; }
    public int? ImageId { get; set; }
    public List<PositionDto>? Positions { get; set; } = new List<PositionDto>();
}