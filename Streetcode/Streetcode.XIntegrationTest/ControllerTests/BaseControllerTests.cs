using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace Streetcode.XIntegrationTest.ControllerTests
{
    
    public class BaseControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        protected HttpClient _client;

        protected string baseUrl = "https://localhost:5001/";

        public BaseControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .CreateClient() ;

           
        }

       

    }
}
