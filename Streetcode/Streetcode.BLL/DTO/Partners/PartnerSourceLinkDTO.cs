using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDto
{
    public int Id { get; set; }
    public LogoTypeDto LogoType { get; set; }
    public UrlDto TargetUrl { get; set; } = null!;
}