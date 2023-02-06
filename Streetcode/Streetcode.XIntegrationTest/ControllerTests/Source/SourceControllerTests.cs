using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Streetcode.BLL.DTO.Sources;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.WebApi.Controllers.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Source
{
    public class SourceControllerTests : BaseControllerTests, IClassFixture<WebApplicationFactory<Program>>
    {
  

        public SourceControllerTests(WebApplicationFactory<Program> factory) : base(factory)
        {

        }

        [Fact]
        public async Task GetCategoryById_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/Sources/GetCategoryById/1");

            response.EnsureSuccessStatusCode(); 
        }
          
        [Fact]
        public async Task GetCategoryById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/Sources/GetCategoryById/100000");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
       
        [Fact]
        public async Task GetCategoryById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/Sources/GetCategoryById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
    
        [Fact]
        public async Task GetCategoryById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/Sources/GetCategoryById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<SourceLinkCategoryDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);

        }


        [Theory]
        [InlineData(1)]
        public async Task GetCategoriesByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/Sources/GetCategoriesByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<SourceLinkCategoryDTO>>();
            var specificItem = returnedValue.Where(x => x.Id == streetcodeId).FirstOrDefault();
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(specificItem.Id == streetcodeId);
        }

        [Theory]
        [InlineData(1)]
        public async Task GetSubCategoriesByCategoryId_ReturnSuccess(int categoryId)
        {
            var response = await _client.GetAsync($"/api/Sources/GetSubCategoriesByCategoryId/{categoryId}");
            var returnedValue = await response.Content.ReadFromJsonAsync <IEnumerable<SourceLinkSubCategoryDTO>>();
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            

        }

    }
}