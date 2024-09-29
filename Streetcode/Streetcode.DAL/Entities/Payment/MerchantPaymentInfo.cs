using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class MerchantPaymentInfo
    {
        [JsonProperty("destination")]
        public string? Destination { get; set; }
    }
}