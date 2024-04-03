using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.BaseController
{
    public class BaseAuthorizationControllerTests<T> : BaseControllerTests<T>
    {
        protected readonly TokenStorage _tokenStorage;

        public BaseAuthorizationControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl, TokenStorage tokenStorage)
            : base(factory, secondPartUrl)
        {
            _tokenStorage = tokenStorage;
        }

        public override void Dispose()
        {
        }
    }
}
