using System.Text.Json.Serialization;

namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPost
    {
        /// <summary>
        /// The Media's caption text. Not returnable for Media in albums.
        /// </summary>
        [JsonPropertyName("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// The Media's ID.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// The Media's type. Can be IMAGE, VIDEO, or CAROUSEL_ALBUM.
        /// </summary>
        [JsonPropertyName("media_type")]
        public string MediaType { get; set; }

        /// <summary>
        /// The Media's URL.
        /// </summary>
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }

        /// <summary>
        /// The Media's permanent URL.
        /// </summary>
        [JsonPropertyName("permalink")]
        public string Permalink { get; set; }

        /// <summary>
        /// The Media's thumbnail image URL. Only available on VIDEO Media.
        /// </summary>
        [JsonPropertyName("thumbnail_url")]
        public string ThumbnailUrl { get; set; }

        [JsonPropertyName("is_pinned")]
        public bool IsPinned { get; set; }
    }
}
