using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Partners;

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