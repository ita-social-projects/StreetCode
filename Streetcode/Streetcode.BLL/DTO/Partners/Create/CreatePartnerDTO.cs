using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Partners.Create;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners
{
    public class CreatePartnerDTO
    {
        public bool IsKeyPartner { get; set; }
        public bool IsVisibleEverywhere { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TargetUrl { get; set; } = null!;
        public int LogoId { get; set; }
        public string UrlTitle { get; set; } = null!;
        public List<CreatePartnerSourceLinkDTO> PartnerSourceLinks { get; set; } = null!;
        public List<StreetcodeShortDTO> Streetcodes { get; set; } = new List<StreetcodeShortDTO>();
    }
}
