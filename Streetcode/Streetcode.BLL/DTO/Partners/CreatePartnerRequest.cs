using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.DTO.Partners
{
    public class CreatePartnerRequest
    {
        public bool IsKeyPartner { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public CreateUrlRequest TargetUrl { get; set; }
        public List<PartnerSourceLinkDTO>? PartnerSourceLinks { get; set; }
    }
}
