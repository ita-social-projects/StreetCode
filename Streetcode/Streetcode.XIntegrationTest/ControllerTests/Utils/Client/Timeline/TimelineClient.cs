using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline
{
    public class TimelineClient(HttpClient client, string secondPartUrl = "")
        : StreetcodeRelatedBaseClient(client, secondPartUrl)
    {
    }
}
