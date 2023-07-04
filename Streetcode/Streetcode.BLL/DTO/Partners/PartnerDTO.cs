using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerDTO
{
    public int Id { get; set; }
    public bool IsKeyPartner { get; set; }
    public bool IsVisibleEverywhere { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int LogoId { get; set; }
    public UrlDTO? TargetUrl { get; set; }
    public List<PartnerSourceLinkDTO>? PartnerSourceLinks { get; set; }
    public List<StreetcodeShortDTO>? Streetcodes { get; set; }
}