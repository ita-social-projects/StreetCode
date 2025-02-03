using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerDto
{
    public int Id { get; set; }
    public bool IsKeyPartner { get; set; }
    public bool IsVisibleEverywhere { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int LogoId { get; set; }
    public UrlDto? TargetUrl { get; set; }
    public List<PartnerSourceLinkDto> PartnerSourceLinks { get; set; } = new List<PartnerSourceLinkDto>();
    public List<StreetcodeShortDto> Streetcodes { get; set; } = new List<StreetcodeShortDto>();
}