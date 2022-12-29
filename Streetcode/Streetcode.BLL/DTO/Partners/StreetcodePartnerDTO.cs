using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class StreetcodePartnerDTO
{
    public bool IsSponsor { get; set; }
    public int StreetcodeId { get; set; }
    public StreetcodeDTO? Streetcode { get; set; }
    public int PartnerId { get; set; }
    public PartnerDTO Partner { get; set; }
}