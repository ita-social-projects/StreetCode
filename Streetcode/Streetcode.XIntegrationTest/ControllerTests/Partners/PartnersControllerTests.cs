using Streetcode.BLL.DTO.Partners;
using Streetcode.DAL.Entities.Partners;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Partner;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Partners
{
    public class PartnersControllerTests : BaseControllerTests<PartnersClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;
        private Partner _testPartner;

        public PartnersControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Partners")
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this._testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
            this._testPartner = PartnerExtracter.Extract(uniqueId);
        }

        public override void Dispose()
        {
            StreetcodeContentExtracter.Remove(this._testStreetcodeContent);
            PartnerExtracter.Remove(this._testPartner);
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
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Partner expectedPartner = this._testPartner;
            var response = await this.client.GetByIdAsync(expectedPartner.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<PartnerDTO>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedPartner.Id, returnedValue.Id),
                () => Assert.Equal(expectedPartner.Title, returnedValue.Title),
                () => Assert.Equal(expectedPartner.Description, returnedValue.Description),
                () => Assert.Equal(expectedPartner.LogoId, returnedValue.LogoId),
                () => Assert.Equal(expectedPartner.IsKeyPartner, returnedValue.IsKeyPartner),
                () => Assert.Equal(expectedPartner.TargetUrl, returnedValue.TargetUrl.Href),
                () => Assert.Equal(expectedPartner.UrlTitle, returnedValue.TargetUrl.Title));
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
