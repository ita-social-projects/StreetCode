using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Payment
{
    public class BasketOrder
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("qty")]
        public int Qty { get; set; }

        [JsonProperty("sum")]
        public long Sum { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("unit")]
        public string Unit { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("header")]
        public string Header { get; set; }

        [JsonProperty("footer")]
        public string Footer { get; set; }

        [JsonProperty("tax")]
        public List<int> Tax { get; set; }

        [JsonProperty("uktzed")]
        public string Uktzed { get; set; }
    }
}
