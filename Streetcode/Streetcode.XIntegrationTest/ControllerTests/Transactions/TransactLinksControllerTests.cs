using Streetcode.BLL.DTO.Toponyms;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.DTO.Transactions;

namespace Streetcode.XIntegrationTest.ControllerTests.Transactions
{
    public class TransactLinksControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public TransactLinksControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/TransactLinks/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/TransactLinks/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<TransactLinkDTO>>();

            Assert.NotNull(returnedValue);
        }
        [Fact]
        public async Task GetTransactLinksById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/TransactLinks/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetTransactLinksById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/TransactLinks/GetById/-100");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTransactLinksById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/TransactLinks/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<TransactLinkDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);

        }

        [Theory]
        [InlineData(1)]
        public async Task GetTransactLinksByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/TransactLinks/GetByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<TransactLinkDTO>();
          
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(returnedValue.Id == streetcodeId);
        }
    }
}
