using Streetcode.DAL.Enums;

namespace Streetcode.BLL.DTO.Partners
{
    public class CreatePartnerSourceLinkDTO
    {
        public string Title { get; set; }

        public LogoType LogoType { get; set; }

        public string TargetUrl { get; set; }
    }
}
