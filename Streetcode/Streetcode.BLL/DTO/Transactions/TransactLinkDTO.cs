using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.BLL.DTO.Transactions;

public class TransactLinkDTO
{
    public int Id { get; set; }
    public string? Url { get; set; }
    public string? QrCodeUrl { get; set; }
    public int StreetcodeId { get; set; }
}