using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class CoordinateControllerTests: BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public CoordinateControllerTests(CustomWebApplicationFactory<Program> factory):base(factory)
        {

        }

       
        [Theory]
        [InlineData(1)]
        public async Task CoordinateControllerTests_GetGetByStreetcodeId_SuccsessStatusCode(int id)
        {
            var responce = await _client.GetAsync($"/api/Coordinate/GetByStreetcodeId/{id}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<StreetcodeCoordinateDTO>>();
            responce.EnsureSuccessStatusCode();
            Assert.NotNull(returnedValue);
            if (returnedValue.Count() != 0)
            {
                Assert.Equal(id, returnedValue.FirstOrDefault(c=>c.StreetcodeId==id)?.StreetcodeId);
            }
        }
        
        [Fact]
        public async Task CoordinateControllerTests_GetGetByStreetcodeId_BadRequestStatusCode()
        {
            var responce = await _client.GetAsync($"/api/Coordinate/GetByStreetcodeId/-10");

            Assert.False(responce.IsSuccessStatusCode);
        }




    }
}
