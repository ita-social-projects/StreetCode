using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool? IsMain { get; set; }
        public int? ImageId { get; set; }
        public List<TeamMemberLinkDTO> TeamMemberLinks { get; set; } = new List<TeamMemberLinkDTO>();
        public List<PositionDTO> Positions { get; set; } = new List<PositionDTO>();
        public ImageDTO? Image { get; set; }
    }
}
