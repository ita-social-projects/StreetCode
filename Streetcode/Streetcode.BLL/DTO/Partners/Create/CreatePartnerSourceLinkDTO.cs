using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Partners.Create
{
  public class CreatePartnerSourceLinkDto
  {
    public int Id { get; set; }

    public LogoType LogoType { get; set; }

    public string TargetUrl { get; set; } = null!;
  }
}
