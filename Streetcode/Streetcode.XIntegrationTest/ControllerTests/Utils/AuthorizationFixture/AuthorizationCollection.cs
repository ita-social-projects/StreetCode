using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils
{
    [CollectionDefinition("Authorization")]
    public class AuthorizationCollection : ICollectionFixture<TokenStorage>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
