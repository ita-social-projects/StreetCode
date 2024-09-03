using Streetcode.XIntegrationTest.ControllerTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.BaseController
{
    public class BaseAuthorizationControllerTests<T> : BaseControllerTests<T>
    {
        public BaseAuthorizationControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl, TokenStorage tokenStorage)
            : base(factory, secondPartUrl)
        {
            this.TokenStorage = tokenStorage;
        }

        protected TokenStorage TokenStorage { get; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.TokenStorage.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
