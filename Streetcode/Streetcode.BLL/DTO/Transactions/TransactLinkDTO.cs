using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode;

namespace Streetcode.BLL.DTO.Transactions;

public class TransactLinkDTO
{
    public int Id;
    public UrlDTO Url;
    public UrlDTO QrCodeUrl;
    public int StreetcodeId;
    public StreetcodeDTO Streetcode;
}