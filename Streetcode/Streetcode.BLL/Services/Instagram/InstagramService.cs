using Newtonsoft.Json;
using Streetcode.BLL.Interfaces.Instagram;
using Streetcode.BLL.Services.Payment.Exceptions;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Instagram;
using Streetcode.DAL.Entities.Payment;

namespace Streetcode.BLL.Services.Instagram
{
    public class InstagramService : IInstagramService
    {
        public async Task<IEnumerable<InstagramPost>> GetPostsAsync()
        {
            var userId = "";
            var accessToken = "";
            var postLimit = 10;

            using (HttpClient client = new HttpClient())
            {
                var baseAddress = $"https://graph.instagram.com/{userId}/media?fields=id,media_type,media_url,caption&limit=${postLimit}&access_token=${accessToken}";
                HttpResponseMessage response = await client.GetAsync(baseAddress);
                (int Code, string Body) result = (Code: (int)response.StatusCode, Body: await response.Content.ReadAsStringAsync());

                return result.Code switch
                {
                    200 => JsonToObject<IEnumerable<InstagramPost>>(result.Body),
                    400 => throw new InvalidRequestParameterException(JsonToObject<Error>(result.Body)),
                    403 => throw new InvalidTokenException(),
                    _ => throw new NotSupportedException()
                };
            }
        }

        private T JsonToObject<T>(string body)
        {
            return JsonConvert.DeserializeObject<T>(body);
        }
    }
}
