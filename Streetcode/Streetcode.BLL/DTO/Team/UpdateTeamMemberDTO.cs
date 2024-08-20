namespace Streetcode.BLL.DTO.Team
{
    public class UpdateTeamMemberDTO : TeamMemberCreateUpdateDTO<TeamMemberLinkDTO>
    {
        public int Id { get; set; }
    }
}
