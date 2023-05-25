using Newtonsoft.Json;

namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPost
    {
        [JsonProperty("object_id")]
        public int Id { get; set; }

        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        [JsonProperty("caption")]
        public string Description { get; set; }
    }
}
