using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Partners;

public class PartnerSourceLinkDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string LogoUrl { get; set; }
    public string TargetUrl { get; set; }
    public int PartnerId { get; set; }
}