using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Media.Images
{
    public class ImageClient : StreetcodeRelatedBaseClient
    {
        public ImageClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }
    }
}
