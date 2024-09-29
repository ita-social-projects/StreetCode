using System.Text.Json.Serialization;

namespace Streetcode.DAL.Entities.Instagram
{
    public class InstagramPost
    {
        /// <summary>
        /// Gets or sets the Media's caption text.
        /// </summary>
        /// <value> The Media's caption text. Not returnable for Media in albums. </value>
        [JsonPropertyName("caption")]
        public string? Caption { get; set; }

        /// <summary>
        /// Gets or sets the Media's ID.
        /// </summary>
        /// <value> The Media's ID. </value>
        [JsonPropertyName("id")]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Media's type.
        /// </summary>
        /// <value> The Media's type. Can be IMAGE, VIDEO, or CAROUSEL_ALBUM. </value>
        [JsonPropertyName("media_type")]
        public string MediaType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Media's URL.
        /// </summary>
        /// <value> The Media's URL. </value>
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Media's permanent URL.
        /// </summary>
        /// <value> The Media's permanent URL. </value>
        [JsonPropertyName("permalink")]
        public string Permalink { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Media's thumbnail image URL.
        /// </summary>
        /// <value> The Media's thumbnail image URL. Only available on VIDEO Media. </value>
        [JsonPropertyName("thumbnail_url")]
        public string? ThumbnailUrl { get; set; }

        [JsonPropertyName("is_pinned")]
        public bool IsPinned { get; set; }
    }
}
