using Streetcode.BLL.DTO.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.XIntegrationTest.ControllerTests.Toponyms
{
    public class ToponymControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public ToponymControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Toponym/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/Toponym/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<ToponymDTO>>();

            Assert.NotNull(returnedValue);
        }
        [Fact]
        public async Task GetToponymById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Toponym/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetToponymById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Toponym/GetById/-100");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetToponymById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Toponym/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<ToponymDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);

        }

        [Theory]
        [InlineData(1)]
        public async Task GetToponymByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/Toponym/GetByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<ToponymDTO>>();
            var specificItem = returnedValue.Where(x => x.Id == streetcodeId).FirstOrDefault();
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(specificItem.Id == streetcodeId);
        }


    }
}
