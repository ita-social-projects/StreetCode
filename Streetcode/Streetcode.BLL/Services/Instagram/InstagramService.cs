using System.Text.Json;
using Microsoft.Extensions.Options;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.DAL.Entities.Instagram;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        private readonly HttpClient _httpClient;
        private readonly InstagramEnvirovmentVariables _envirovment;
        private readonly string _userId;
        private readonly string _accessToken;

        public InstagramService(IOptions<InstagramEnvirovmentVariables> instagramEnvirovment)
        {
            _httpClient = new HttpClient();
            _envirovment = instagramEnvirovment.Value;
            _userId = _envirovment.InstagramID;
            _accessToken = _envirovment.InstagramToken;
        }

        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            var postLimit = 10;

            string apiUrl = $"https://graph.instagram.com/{_userId}/media?fields=id,caption,media_type,media_url,permalink,thumbnail_url&limit={postLimit}&access_token={_accessToken}";

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            string jsonResponse = await response.Content.ReadAsStringAsync();

            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            };

            var postResponse = JsonSerializer.Deserialize<InstagramPostResponse>(jsonResponse, jsonOptions);

            IEnumerable<InstagramPost> posts = postResponse.Data.OrderByDescending(p => p.IsPinned);

            return posts;
        }
    }
}