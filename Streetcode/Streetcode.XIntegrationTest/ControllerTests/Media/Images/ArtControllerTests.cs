using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Media.Images
{
    public class ArtControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        string secondPartUrl = "/api/Art";
        public ArtControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task ArtControllerTests_GetAllSuccessfulResult()
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/GetAll");
            Assert.True(responce.IsSuccessStatusCode);
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<ArtDTO>>();

            responce.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotNull(returnedValue);

        }
        [Fact]
        public async Task ArtControllerTests_GetByIdSuccessfulResult()
        {
            int id = 1;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            var returnedValue = await responce.Content.ReadFromJsonAsync<ArtDTO>();

            Assert.Equal(id, returnedValue?.Id);
            Assert.True(responce.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ArtControllerTests_GetByIdIncorectBadRequest()
        {
            int id = -100;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, responce.StatusCode);
            Assert.False(responce.IsSuccessStatusCode);
        }


      
        [Theory]
        [InlineData(1)]
        public async Task ArtControllerTests_GetByStreetcodeIdSuccessfulResult(int streetcodeId)
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable< ArtDTO>>();
            Assert.True(responce.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(t => t.Streetcodes.Any(s => s.Id == streetcodeId)));
           
        }

    }
}
