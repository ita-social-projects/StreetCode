namespace Streetcode.XIntegrationTest.ControllerTests
{
    using Streetcode.XIntegrationTest.ControllerTests.Utils;
    using Xunit;

    public class BaseControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        protected StreetcodeClient client;

        public BaseControllerTests(CustomWebApplicationFactory<Program> factory,string secondPartUrl = "")
        {
            this.client = new StreetcodeClient(factory.CreateClient(), secondPartUrl);
        }
    }
}
