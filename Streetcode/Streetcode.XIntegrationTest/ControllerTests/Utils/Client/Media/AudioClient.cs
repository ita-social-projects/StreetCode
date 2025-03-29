using RestSharp;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media
{
    public class AudioClient : StreetcodeRelatedBaseClient
    {
        public AudioClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }

        public async Task<RestResponse> GetBaseAudio(int id)
        {
            return await SendQuery($"/getBaseAudio/{id}");
        }

        public async Task<RestResponse> CreateAudio(AudioFileBaseCreateDTO audioCreateDto, string authToken = "")
        {
            return await SendCommand($"/create", Method.Post, audioCreateDto, authToken);
        }

        public async Task<RestResponse> UpdateAudio(AudioFileBaseUpdateDTO audioUpdateDto, string authToken = "")
        {
            return await SendCommand("/update", Method.Put, audioUpdateDto, authToken);
        }

        public async Task<RestResponse> DeleteAudio(int id, string authToken = "")
        {
            return await SendCommand($"/delete/{id}", Method.Delete, authToken);
        }
    }
}
