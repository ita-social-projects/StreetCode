using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent
{
    public class TextControllerTests : BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
    {
        public TextControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Text/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/Text/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<TextDTO>>();

            Assert.NotNull(returnedValue);
        }
        [Fact]
        public async Task GetTextById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Text/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetTextById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Text/GetById/100000");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTextById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Text/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<TextDTO>();

            Assert.Equal(id, returnedValue.Id);
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetTextByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/Text/GetByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<TextDTO>();
            
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(returnedValue.Id == streetcodeId);
            Assert.Equal("Дитинство та юність", returnedValue.Title);
        }




    }
}
