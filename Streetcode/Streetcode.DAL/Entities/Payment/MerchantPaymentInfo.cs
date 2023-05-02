using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class MerchantPaymentInfo
    {
        // [JsonProperty("reference")]
        // public string Reference { get; set; }

        [JsonProperty("destination")]
        public string Destination { get; set; }

        // [JsonProperty("basketOrder")]
        // public List<BasketOrder> BasketOrder { get; set; }
    }
}