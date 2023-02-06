using FluentResults;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    public class StreetcodeControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetAllStreetcodes_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Streetcode/GetAll");

            response.EnsureSuccessStatusCode();
        }

       
        [Fact]
        public async Task GetStreetcodeById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Streetcode/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
        [Fact]
        public async Task GetStreetcodeById_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Streetcode/GetById/1");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetStreetcodeById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Streetcode/GetById/100000");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        //[Fact]
        //public async Task GetStreetcodes_ReturnContent()
        //{
        //    var response = await _client.GetAsync("/api/Streetcode/GetAll");

        //    var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<StreetcodeDTO>>();

        //    Assert.NotNull(returnedValue);
        //}
        //[Fact]
        //public async Task GetStreetcodetById_ReturnSuccessContent()
        //{
        //    int id = 1;
        //    var response = await _client.GetAsync($"/api/Streetcode/GetById/{id}");

        //    var returnedValue = await response.Content.ReadFromJsonAsync<StreetcodeDTO>();

        //    Assert.Equal(id, returnedValue.Id);
        //    Assert.True(response.IsSuccessStatusCode);
        //}
        //[Theory]
        //[InlineData(1)]
        //public async Task GeStreetcodeByIndex_ReturnSuccess(int index)
        //{
        //    var response = await _client.GetAsync($"/api/Streetcode/GetByIndex/{index}");
        //    var returnedValue = await response.Content.ReadFromJsonAsync<StreetcodeDTO>();     
        //    Assert.NotNull(returnedValue);
        //    Assert.True(response.IsSuccessStatusCode);
        //    Assert.True(returnedValue.Index == index);
        //}

    }
}
