using Microsoft.AspNetCore.Mvc.Testing;
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
    public class ImageControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        string secondPartUrl = "/api/Image";
        public ImageControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }
        [Fact]
        public async Task ImageControllerTests_GetAllSuccsesfulResult()
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/GetAll");
            Assert.True(responce.IsSuccessStatusCode);
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<ImageDTO>>();

            responce.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.NotNull(returnedValue);

        }
        [Fact]
        public async Task ImageControllerTests_GetByIdSuccsesfulResult()
        {
            int id = 1;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            var returnedValue = await responce.Content.ReadFromJsonAsync<ImageDTO>();

            Assert.Equal(id, returnedValue?.Id);
            Assert.True(responce.IsSuccessStatusCode);
        }

        [Fact]
        public async Task ImageControllerTests_GetByIdIncorectBadRequest()
        {
            int id = -100;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, responce.StatusCode);
            Assert.False(responce.IsSuccessStatusCode);
        }


        //imagedto has nullable streetcode 
        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        public async Task ImageControllerTests_GetByStreetcodeIdSuccsesfulResult(int streetcodeId)
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<ImageDTO>>();

            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(i => i.Streetcodes.Any(s=>s.Id==streetcodeId)));
            Assert.True(responce.IsSuccessStatusCode);
        }

    }
}
