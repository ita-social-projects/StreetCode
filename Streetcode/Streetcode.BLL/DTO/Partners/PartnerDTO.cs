using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{
    public int Id { get; set; }
    public bool IsKeyPartner { get; set; }
    public int ImageId { get; set; }
    public string? Description { get; set; }
    public string LogoUrl { get; set; }
    public UrlDTO TargetUrl { get; set; }
    public List<StreetcodeDTO> StreetcodePartners { get; set; }
    public List<PartnerSourceLinkDTO> PartnerSourceLinks { get; set; }
}