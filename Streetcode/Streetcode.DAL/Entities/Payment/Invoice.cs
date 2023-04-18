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

       // Not sure if constructor really needed as this is an input model used to send data.

        /// <summary>
        /// Gets the amount of the transaction in the minimum units (coins, cents) of the account currency.
        /// </summary>
        [JsonProperty("amount")]
        public long Amount { get; set; }

        /// <summary>
        /// Gets or sets the currency code according to ISO 4217. By default it is 980 which is Ukrainian Hryvnia.
        /// </summary>
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