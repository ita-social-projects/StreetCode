using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    public class RelatedFigureControllerTests : BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
    {
        public RelatedFigureControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Theory]
        [InlineData(2)]
        public async Task GetRelatedFigureByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/RelatedFigure/{streetcodeId}");
            //var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<RelatedFigureDTO>>();
            //var specificItem = returnedValue.Where(x => x.Id == streetcodeId).FirstOrDefault();         
            Assert.True(response.IsSuccessStatusCode);          
        }
    }
}
