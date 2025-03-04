using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline
{
    public class TimelineClient : StreetcodeRelatedBaseClient
    {
        public TimelineClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }
    }
}
