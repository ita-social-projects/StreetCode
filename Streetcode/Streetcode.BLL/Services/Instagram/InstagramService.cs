using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly InstagramEnvirovmentVariables _envirovment;
        private readonly string _userId;
        private readonly string _accessToken;
        private static int postLimit = 10;

        public InstagramService(IOptions<InstagramEnvirovmentVariables> instagramEnvirovment, IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _envirovment = instagramEnvirovment.Value;
            _userId = _envirovment.InstagramID;
            _accessToken = _envirovment.InstagramToken;
        }

        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            var apiUrl = $"https://graph.instagram.com/{_userId}/media?fields=id,caption,media_type,media_url,permalink,thumbnail_url&limit={2 * postLimit}&access_token={_accessToken}";

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var postResponse = JsonSerializer.Deserialize<InstagramPostResponse>(jsonResponse, jsonOptions);

            var posts = RemoveVideoMediaType(postResponse!.Data);

            return posts;
        }

        public IEnumerable<InstagramPost> RemoveVideoMediaType(IEnumerable<InstagramPost> posts)
        {
            return posts.Where(p => p.MediaType != "VIDEO").Take(postLimit);
        }
    }
}