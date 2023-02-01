using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDTO
{
    public int Id { get; set; }
    public LogoTypeDTO LogoTpe { get; set; }
    public UrlDTO TargetUrl { get; set; }
}