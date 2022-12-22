using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class StreetcodePartnerDTO
{
    public int Id;
    public string Title;
    public bool IsSponsor;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
    public int PartnerId;
    public PartnerDTO Partner;
}