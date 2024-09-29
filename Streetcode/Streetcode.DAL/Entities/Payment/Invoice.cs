using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class Invoice
    {
        public Invoice(long amount, int? ccy, MerchantPaymentInfo merchantPaymentInfo, string? redirectUrl)
         {
            Amount = amount;
            Ccy = ccy;
            MerchantPaymentInfo = merchantPaymentInfo;
            RedirectUrl = redirectUrl;
        }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("ccy")]
        public int? Ccy { get; set; }

        [JsonProperty("merchantPaymInfo")]
        public MerchantPaymentInfo MerchantPaymentInfo { get; set; }

        [JsonProperty("redirectUrl")]
        public string? RedirectUrl { get; set; }
    }
}
