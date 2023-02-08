using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Transactions;

public class TransactLinkDTO
{
    public int Id { get; set; }
    public UrlDTO Url { get; set; }
    public UrlDTO QrCodeUrl { get; set; }
    public int StreetcodeId { get; set; }
}