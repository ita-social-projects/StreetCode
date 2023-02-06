using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.AdditionalContent
{
    public class SubtitleControllerTests: BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public SubtitleControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task SubtitleControllerTests_GetAllSuccessfulResult()
        {
            var responce = await _client.GetAsync($"/api/Subtitle/GetAll");
            Assert.True(responce.IsSuccessStatusCode);
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<SubtitleDTO>>();

            Assert.NotNull(returnedValue);
           
        }

        [Theory]
        [InlineData(1)]
        public async Task SubtitleControllerTests_GetByIdSuccessfulResult(int id)
        {
            var responce = await _client.GetAsync($"/api/Subtitle/getById/{id}");

            var returnedValue = await responce.Content.ReadFromJsonAsync<SubtitleDTO>();

            Assert.Equal(id, returnedValue.Id);
            Assert.True(responce.IsSuccessStatusCode);
        }

        [Fact]
        public async Task SubtitleControllerTests_GetByIdIncorrectBadRequestResult()
        {
            var responce = await _client.GetAsync($"/api/Subtitle/getById/-100");
            Assert.False(responce.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData(1)]
        public async Task SubtitleControllerTests_GetByStreetcodeIdSuccessfulResult(int streetcodeId)
        {
            var responce = await _client.GetAsync($"/api/Subtitle/getByStreetcodeId/{streetcodeId}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<SubtitleDTO>>();

            Assert.NotNull(returnedValue);
            Assert.True( returnedValue.All(s=>s.StreetcodeId==streetcodeId));
            Assert.True(responce.IsSuccessStatusCode);
        }


    }
}
