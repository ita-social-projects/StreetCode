using System.Net;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Timeline;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Timeline
{
    [Collection("TimelineItem")]
    public class TimelineItemControllerTests : BaseControllerTests<TimelineClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly TimelineItem _timelineItem;
        private readonly StreetcodeContent _testStreetcodeContent;

        public TimelineItemControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/TimelineItem")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _timelineItem = TimelineItemExtracter.Extract(uniqueId);
            _testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task GetAll_ReturnsSuccessStatusCode()
        {
            // Act
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TimelineItemDTO>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnsSuccessStatusCode()
        {
            // Arrange
            TimelineItem expectedTimeline = _timelineItem;

            // Act
            var response = await this.Client.GetByIdAsync(expectedTimeline.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<TimelineItemDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedTimeline.Id, returnedValue.Id),
                () => Assert.Equal(expectedTimeline.Title, returnedValue.Title));
        }

        [Fact]
        public async Task GetByIdIncorrect_ReturnsBadRequest()
        {
            // Arrange
            int incorrectId = -1;

            // Act
            var response = await this.Client.GetByIdAsync(incorrectId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnsSuccessStatusCode()
        {
            // Arrange
            int streetcodeId = _testStreetcodeContent.Id;

            // Act
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<TimelineItemDTO>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
        }

        [Fact(Skip = "will fail until pr 2098 is merged")]
        public async Task GetByStreetcodeIdIncorrect_ReturnsBadRequest()
        {
            // Assert
            int incorrectStreetcodeId = -1;

            // Act
            var response = await this.Client.GetByStreetcodeId(incorrectStreetcodeId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        
    }
}
