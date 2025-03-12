﻿using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.AuthorizationFixture;

namespace Streetcode.XIntegrationTest.ControllerTests.BaseController
{
    public class BaseAuthorizationControllerTests<T> : BaseControllerTests<T>
    {
        public BaseAuthorizationControllerTests(CustomWebApplicationFactory<Program> factory, string secondPartUrl, TokenStorage tokenStorage)
            : base(factory, secondPartUrl)
        {
            this.TokenStorage = tokenStorage;
        }

        protected TokenStorage TokenStorage { get; init; }
    }
}
