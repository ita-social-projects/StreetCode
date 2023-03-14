using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;

namespace Streetcode.BLL.DTO.Partners
{
    public class CreatePartnerDTO
    {
        public int Id { get; set; }
        public bool IsKeyPartner { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string TargetUrl { get; set; }
        public int LogoId { get; set; }
        public string? UrlTitle { get; set; }
        public string LogoBase64 { get; set; }
        public List<CreatePartnerSourceLinkDTO>? PartnerSourceLinks { get; set; }
    }
}
