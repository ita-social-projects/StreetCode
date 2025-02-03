using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Toponym
{
    public class ToponymsClient : StreetcodeRelatedBaseClient
    {
        public ToponymsClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }
    }
}
