using System.Net;
using Streetcode.BLL.DTO.Event;
using Streetcode.BLL.DTO.Event.CreateUpdate;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Event;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Event
{
    [Collection("Authorization")]
    public class EventControllerTests : BaseControllerTests<EventClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly EventDTO _testEvent;
        private readonly TokenStorage _tokenStorage;

        public EventControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Events")
        {
            _tokenStorage = tokenStorage;
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _testEvent = new EventDTO
            {
                Id = uniqueId,
                Title = $"Test Event {uniqueId}",
                EventType = "Historical"
            };
        }

        [Fact]
        public async Task GetAll_ReturnsSuccess()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllEventsResponseDTO>(response.Content);

            Assert.Multiple(() =>
            {

                Assert.True(response.IsSuccessStatusCode, $"Expected success status code, but got {response.StatusCode}. Response: {response.Content}");
                Assert.NotNull(returnedValue);
            });
        }

        [Fact]
        public async Task GetById_ReturnsSuccess()
        {
            var createResponse = await this.Client.CreateAsync(new CreateUpdateEventDTO
            {
                Title = _testEvent.Title,
                EventType = _testEvent.EventType
            }, _tokenStorage.AdminAccessToken);

            Assert.True(createResponse.IsSuccessStatusCode, $"Failed to create event. Status: {createResponse.StatusCode}, Response: {createResponse.Content}");

            var createContent = createResponse.Content;
            if (string.IsNullOrEmpty(createContent))
            {
                Assert.Fail("Create response content is empty.");
            }

            if (!int.TryParse(createContent, out int createdEventId))
            {
                Assert.Fail($"Failed to parse created event ID from response. Content: {createContent}");
            }

            var response = await this.Client.GetByIdAsync(createdEventId);
            Assert.True(response.IsSuccessStatusCode,
                $"Expected success status code, but got {response.StatusCode}. Response: {response.Content}");

            var responseContent = response.Content;
            if (string.IsNullOrEmpty(responseContent))
            {
                Assert.Fail("GetById response content is empty.");
            }

            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<EventDTO>(responseContent);
            if (returnedValue == null)
            {
                Assert.Fail($"Failed to deserialize GetById response. Content: {responseContent}");
            }

            Assert.Equal(createdEventId, returnedValue.Id);
        }

        [Fact]
        public async Task Create_ReturnsSuccess_WithAdminToken()
        {
            var createDto = new CreateUpdateEventDTO { Title = "New Event", EventType = "Custom" };
            var response = await this.Client.CreateAsync(createDto, _tokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsUnauthorized_WithoutToken()
        {
            var createDto = new CreateUpdateEventDTO { Title = "New Event", EventType = "Custom" };
            var response = await this.Client.CreateAsync(createDto);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_ReturnsSuccess_WithAdminToken()
        {
            var createResponse = await this.Client.CreateAsync(new CreateUpdateEventDTO
            {
                Title = _testEvent.Title,
                EventType = _testEvent.EventType
            }, _tokenStorage.AdminAccessToken);

            Assert.True(createResponse.IsSuccessStatusCode,
                $"Failed to create event. Status: {createResponse.StatusCode}, Response: {createResponse.Content}");

            var createContent = createResponse.Content;
            if (string.IsNullOrEmpty(createContent))
            {
                Assert.Fail("Create response content is empty.");
            }

            if (!int.TryParse(createContent, out int createdEventId))
            {
                Assert.Fail($"Failed to parse created event ID from response. Content: {createContent}");
            }

            if (_tokenStorage.AdminAccessToken == null)
            {
                Assert.Fail("Admin token is null");
            }

            var response = await this.Client.DeleteAsync(createdEventId, _tokenStorage.AdminAccessToken);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
