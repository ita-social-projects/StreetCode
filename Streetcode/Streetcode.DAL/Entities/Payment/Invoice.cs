using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class Invoice
    {
        public Invoice(long amount, int? ccy, MerchantPaymentInfo merchantPaymentInfo, string redirectUrl)
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
        public string RedirectUrl { get; set; }

        // [JsonProperty("webHookUrl")]
        // public string WebhookUrl { get; set; }

        // [JsonProperty("validity")]
        // public long Validity { get; set; }

        // [JsonProperty("paymentType")]
        // public string PaymentType { get; set; }

        // [JsonProperty("qrId")]
        // public string QrId { get; set; }

        // [JsonProperty("saveCardData")]
        // public SaveCardData SaveCardData { get; set; }
    }
}
