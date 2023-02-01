using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class CoordinateControllerTests: BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
    {
        public CoordinateControllerTests(WebApplicationFactory<Program> factory):base(factory)
        {

        }

       
        [Fact]
        public async Task GetId()
        {

            var responce = await _client.GetAsync("/api/Coordinate/GetByStreetcodeId/1");
            //https://localhost:5001/api/Coordinate/GetByStreetcodeId/1

            // Assert
            responce.EnsureSuccessStatusCode(); // Status Code 200-299
        }
        


    }
}
