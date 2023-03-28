using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Partners
{
    public class CreatePartnerSourceLinkDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public LogoType LogoType { get; set; }

        public string TargetUrl { get; set; }
    }
}
