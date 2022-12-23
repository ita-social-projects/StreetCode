using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDTO
{
    public int Id;
    public string LogoUrl;
    public UrlDTO Url;
    public int PartnerId;
    public PartnerDTO Partner;
}