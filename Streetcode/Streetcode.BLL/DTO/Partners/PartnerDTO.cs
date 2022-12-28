using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Media.Images;
namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{
    public int Id { get; set; }
    public ImageDTO Image { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public UrlDTO TargetUrl { get; set; }
    public List<StreetcodePartnerDTO> StreetcodePartners { get; set; }
    public List<PartnerSourceLinkDTO> PartnerSourceLinks { get; set; }
}