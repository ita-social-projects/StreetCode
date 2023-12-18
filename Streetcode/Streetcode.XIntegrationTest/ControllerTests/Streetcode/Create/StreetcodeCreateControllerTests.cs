using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.Create
{
    public class StreetcodeCreateControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeCreateControllerTests(CustomWebApplicationFactory<Program> factory)
           : base(factory, "/api/Streetcode")
        {

        }

        [Fact]
        [ExtractCreateTestStreetcode]
        public async Task Create_ReturnsSuccessStatusCode()
        {
            // Arrange
            var streetcodeCreateDTO = ExtractCreateTestStreetcode.StreetcodeForTest;

            // Act
            var response = await client.CreateAsync(streetcodeCreateDTO);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }
    }
}
