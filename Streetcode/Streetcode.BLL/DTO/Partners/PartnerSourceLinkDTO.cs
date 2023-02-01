using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public LogoTypeDTO LogoTpe { get; set; }
    public string TargetUrl { get; set; }
}