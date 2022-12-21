using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Media.Images;
namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{
    public int Id;
    public ImageDTO Image;
    public string Description;
    public string LogoUrl;
    public UrlDTO TargetUrl;
    public List<StreetcodePartnerDTO> StreetcodePartners;
    public List<PartnerSourceLinkDTO> PartnerSourceLinks;
}