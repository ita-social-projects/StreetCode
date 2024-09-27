using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Streetcode.BLL.DTO.Media.Images;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberCreateDTO : TeamMemberCreateUpdateDTO
    {
        public List<TeamMemberLinkCreateDTO>? TeamMemberLinks { get; set; } = new List<TeamMemberLinkCreateDTO>();
    }
}
