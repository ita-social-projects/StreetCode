using Microsoft.AspNetCore.Mvc.Testing;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.DAL.Entities.Streetcode.TextContent;
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
    public class TagControllerTests :BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        string secondPartUrl = "/api/Tag";
        public TagControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task TagControllerTests_GetAllSuccessfulResult()
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/GetAll");
            Assert.True(responce.IsSuccessStatusCode);

            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<TagDTO>>();

            Assert.NotNull(returnedValue);

        }
        [Fact]
        public async Task TagControllerTests_GetByIdSuccessfulResult()
        {
            int id = 1;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            var returnedValue = await responce.Content.ReadFromJsonAsync<TagDTO>();

            Assert.Equal(id, returnedValue?.Id);
            Assert.True(responce.IsSuccessStatusCode);
        }

        [Fact]
        public async Task TagControllerTests_GetByIdIncorectBadRequest()
        {
            int id = -100;
            var responce = await _client.GetAsync($"{secondPartUrl}/getById/{id}");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest,responce.StatusCode);
            Assert.False(responce.IsSuccessStatusCode);
        }

       
        //dont return streetcodeenumerable
        [Theory]
        [InlineData(1)]
        public async Task TagControllerTests_GetByStreetcodeIdSuccessfulResult(int streetcodeId)
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<IEnumerable<TagDTO>>();

            Assert.NotNull(returnedValue);
            Assert.True(returnedValue.All(t => t.Streetcodes.Any(s=>s.Id==streetcodeId)));
            Assert.True(responce.IsSuccessStatusCode);
        }
        [Fact]
        public async Task TagControllerTests_GetByStreetcodeIdIncorectBadRequest()
        {
            int streetcodeId = -100;
            var responce = await _client.GetAsync($"{secondPartUrl}/getByStreetcodeId/{streetcodeId}");

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, responce.StatusCode);
            Assert.False(responce.IsSuccessStatusCode);
        }

        //return only with the equaltitle
        [Theory]
        [InlineData("writer")]
        public async Task TagControllerTests_GetByTitleSuccessfulResult(string title)
        {
            var responce = await _client.GetAsync($"{secondPartUrl}/GetTagByTitle/{title}");
            var returnedValue = await responce.Content.ReadFromJsonAsync<TagDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(returnedValue.Title,title);
            Assert.True(responce.IsSuccessStatusCode);

        }


        [Fact]
        public async Task TagControllerTests_GetByTitleIncorrectBadRequest()
        {
            string title = "Some_incorect_Title";
            var responce = await _client.GetAsync($"{secondPartUrl}/GetTagByTitle/{title}");
            Assert.False(responce.IsSuccessStatusCode);
        }



    }
}
