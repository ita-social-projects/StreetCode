using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Partners
{
    public class PartnersControllerTests : BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;

        public PartnersControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Partners")
        {
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(this.GetHashCode(), Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PartnerDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        [ExtractTestPartners]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Partner expected = ExtractTestPartners.PartnerForTest;
            var response = await this.client.GetByIdAsync(expected.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<PartnerDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expected.Id, returnedValue.Id),
                () => Assert.Equal(expected.Title, returnedValue.Title),
                () => Assert.Equal(expected.Description, returnedValue.Description),
                () => Assert.Equal(expected.LogoId, returnedValue.LogoId),
                () => Assert.Equal(expected.IsKeyPartner, returnedValue.IsKeyPartner),
                () => Assert.Equal(expected.TargetUrl, returnedValue.TargetUrl.Href),
                () => Assert.Equal(expected.UrlTitle, returnedValue.TargetUrl.Title));
        }

        [Fact]
        public async Task PartnersControllerTests_GetByIdIncorrectReturnBadRequest()
        {
            int id = -100;
            var response = await this.client.GetByIdAsync(id);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            int streetcodeId = this._testStreetcodeContent.Id;

            var response = await this.client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PartnerDTO>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                          () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                          () => Assert.False(response.IsSuccessStatusCode));
        }
    }
}
