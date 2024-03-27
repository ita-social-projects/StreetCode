using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Additional
{
    public class SubtitleClient : StreetcodeRelatedBaseClient
    {
        public SubtitleClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }
    }
}
