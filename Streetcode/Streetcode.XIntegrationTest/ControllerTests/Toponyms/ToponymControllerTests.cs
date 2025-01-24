using System.Net;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Toponyms;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Toponym;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Toponyms;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Toponyms
{
    [Collection("Toponym")]
    public class ToponymControllerTests : BaseControllerTests<ToponymsClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly Toponym toponym;
        private readonly StreetcodeContent testStreetcodeContent;

        public ToponymControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "api/Toponym")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.toponym = ToponymExtracter.Extract(uniqueId);
            this.testStreetcodeContent = StreetcodeContentExtracter
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
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllToponymsResponseDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnsSuccessStatusCode()
        {
            // Arrange
            Toponym expectedToponym = this.toponym;

            // Act
            var response = await this.Client.GetByIdAsync(expectedToponym.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<ToponymDTO>(response.Content);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedToponym.Id, returnedValue.Id),
                () => Assert.Equal(expectedToponym.Oblast, returnedValue.Oblast));
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
            int streetcodeId = this.testStreetcodeContent.Id;

            // Act
            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<ToponymDTO>>(response.Content);

            // Assert
            Assert.True(response.IsSuccessful);
            Assert.NotNull(returnedValue);
        }

        [Fact]
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
