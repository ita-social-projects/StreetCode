using Microsoft.AspNetCore.Mvc.Testing;
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

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.TextContent
{
    public class TermControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public TermControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        
        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Term/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/Term/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<TermDTO>>();

            Assert.NotNull(returnedValue);
        }
        
        [Fact]
        public async Task GetTermtById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Term/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);

        }

        [Fact]
        public async Task GetTermById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Term/GetById/100000");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        
        [Fact]
        public async Task GetTermById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Term/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<TermDTO>();            

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);
            Assert.Equal("етнограф", returnedValue.Title);

        }

     
    }
}
