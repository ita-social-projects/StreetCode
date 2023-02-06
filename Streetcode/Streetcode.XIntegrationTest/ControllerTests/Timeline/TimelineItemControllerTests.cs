using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Streetcode.BLL.DTO.Timeline;

namespace Streetcode.XIntegrationTest.ControllerTests.Timeline
{
    public class TimelineItemControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public TimelineItemControllerTests(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
            
        }
        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/TimelineItem/GetAll");

            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task GetAll_ReturnContent()
        {
            var response = await _client.GetAsync("/api/TimelineItem/GetAll");

            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<TimelineItemDTO>>();

            Assert.NotNull(returnedValue);
        }
        [Fact]
        public async Task GetTimelineById_ReturnsExpectedMediaType()
        {
            var response = await _client.GetAsync("/api/TimelineItem/GetById/1");

            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }

        [Fact]
        public async Task GetTimelinetById_ReturnsNotFoundStatusCode()
        {
            var response = await _client.GetAsync("/api/TimelineItem/GetById/-100");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetTimelineById_ReturnSuccessContent()
        {
            int id = 1;
            var response = await _client.GetAsync($"/api/TimelineItem/GetById/{id}");

            var returnedValue = await response.Content.ReadFromJsonAsync<TimelineItemDTO>();

            Assert.NotNull(returnedValue);
            Assert.Equal(id, returnedValue?.Id);

        }

        [Theory]
        [InlineData(1)]
        public async Task GetTimelineByStreetcodeId_ReturnSuccess(int streetcodeId)
        {
            var response = await _client.GetAsync($"/api/TimelineItem/GetByStreetcodeId/{streetcodeId}");
            var returnedValue = await response.Content.ReadFromJsonAsync<IEnumerable<TimelineItemDTO>>();
            var specificItem = returnedValue.Where(x => x.Id == streetcodeId).FirstOrDefault();
            Assert.NotNull(returnedValue);
            Assert.True(response.IsSuccessStatusCode);
            Assert.True(specificItem.Id == streetcodeId);
        }
    }
   
}
