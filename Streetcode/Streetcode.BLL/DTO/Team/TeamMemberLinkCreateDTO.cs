using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.DTO.Team
{
    public class TeamMemberLinkCreateDTO : TeamMemberLinkCreateUpdateDTO
    {
        public LogoTypeDTO LogoType { get; set; }
        public string TargetUrl { get; set; }
        public int TeamMemberId { get; set; }
    }
}
