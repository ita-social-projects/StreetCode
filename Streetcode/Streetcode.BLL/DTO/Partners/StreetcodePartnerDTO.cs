using Streetcode.BLL.DTO.Identifier;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Partners;

public class StreetcodePartnerDTO
{
    public IdentifierDTO Identifier;
    public bool IsSponsor;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
    public int PartnerId;
    public PartnerDTO Partner;
}