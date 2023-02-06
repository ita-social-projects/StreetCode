using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json;
using System.Net.Http.Json;
using Streetcode.BLL.DTO.Sources;
using Streetcode.XIntegrationTest.ControllerTests.Utils;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent
{
    public class FactControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public FactControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Fact/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/Fact/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<FactDTO>>();

            Assert.NotNull(returnedValue);
        }
        [Fact]
        public async Task GetFactById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Fact/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
        
        [Fact]
        public async Task GetFactById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Fact/GetById/100000");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task GetFactById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Fact/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<FactDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);

        }
        
        [Theory]
        [InlineData(1)]
        public async Task GetFactsByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/Fact/GetByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<FactDTO>>();
            var specificItem = returnedValue.Where(x => x.Id == streetcodeId).FirstOrDefault();
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(specificItem.Id == streetcodeId);
        }

    }
}
