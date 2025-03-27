using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Transaction
{
    public class TransactLinksClient : StreetcodeRelatedBaseClient
    {
        public TransactLinksClient(HttpClient client, string secondPartUrl = "")
            : base(client, secondPartUrl)
        {
        }
    }
}
