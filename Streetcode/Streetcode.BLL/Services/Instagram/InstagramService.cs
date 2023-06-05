using System.Text.Json;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramEnvirovmentVariables _envirovment;
        private readonly string _userId;
        private readonly string _accessToken;
        private static int postLimit = 10;

        public InstagramService(IOptions<InstagramEnvirovmentVariables> instagramEnvirovment)
        {
            _httpClient = new HttpClient();
            _envirovment = instagramEnvirovment.Value;
            _userId = _envirovment.InstagramID;
            _accessToken = _envirovment.InstagramToken;
        }

        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            string apiUrl = $"https://graph.instagram.com/{_userId}/media?fields=id,caption,media_type,media_url,permalink,thumbnail_url&limit={2 * postLimit}&access_token={_accessToken}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };

            var postResponse = JsonSerializer.Deserialize<InstagramPostResponse>(jsonResponse, jsonOptions);

            IEnumerable<InstagramPost> posts = RemoveVideoMediaType(postResponse.Data);

            return posts;
        }

        public IEnumerable<InstagramPost> RemoveVideoMediaType(IEnumerable<InstagramPost> posts)
        {
            return posts.Where(p => p.MediaType != "VIDEO").Take(postLimit);
        }
    }
}