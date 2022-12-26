using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDTO
{
    public int Id { get; set; }
    public string LogoUrl { get; set; }
    public UrlDTO Url { get; set; }
    public int PartnerId { get; set; }
    public PartnerDTO Partner { get; set; }
}