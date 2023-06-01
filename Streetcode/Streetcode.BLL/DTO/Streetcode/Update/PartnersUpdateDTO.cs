using Streetcode.BLL.DTO.Streetcode.Update.Interface;

namespace Streetcode.BLL.DTO.Streetcode.Update
{
    public class PartnersUpdateDTO : IChanged
    {
        public int StreetcodeId { get; set; }
        public int PartnerId { get; set; }
        public bool? IsChanged { get; set; }
    }
}
