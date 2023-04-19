using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class InvoiceInfo
    {
        [JsonConstructor]
        public InvoiceInfo(string invoiceId, string pageUrl)
        {
            InvoiceId = invoiceId;
            PageUrl = pageUrl;
        }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; }

        [JsonProperty("pageUrl")]
        public string PageUrl { get; }
    }
}
