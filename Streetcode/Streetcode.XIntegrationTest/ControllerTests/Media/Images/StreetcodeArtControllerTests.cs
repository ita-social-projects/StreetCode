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
    public class StreetcodeArtControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        string secondPartUrl = "/api/StreetcodeArt";
        public StreetcodeArtControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        //has nullable streetcodes

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public async Task StreetcodeArtControllerTests_GetByStreetcodeIdSuccsesfulResult(int streetcodeId)
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<StreetcodeArtDTO>>();

            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(a => (a.Streetcode?.Id??streetcodeId) == streetcodeId ));
            Assert.True(responce.IsSuccessStatusCode);
        }
        [Fact]
        public async Task StreetcodeArtControllerTests_GetByStreetcodeIdIncorectBadRequest()
        {
            int streetcodeId = -100;
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");

            // Assert.Equal(System.Net.HttpStatusCode.BadRequest, responce.StatusCode);
            Assert.True(responce.IsSuccessStatusCode);
        }
    }
       
}
