using System.Net;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Shared;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Partner;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Partners
{
    [Collection("Authorization")]
    public class PartnersControllerTests : BaseControllerTests<PartnersClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly StreetcodeContent testStreetcodeContent;
        private readonly TokenStorage tokenStorage;
        private readonly Partner testPartner;

        public PartnersControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Partners")
        {
            this.tokenStorage = tokenStorage;
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            this.testStreetcodeContent = StreetcodeContentExtracter
                .Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
            this.testPartner = PartnerExtracter.Extract(uniqueId);
        }

        [Fact]
        public async Task СreatePartner_ReturnBadRequest_WhenValidationFailed()
        {
            var expectedError = "Поле 'Назва' не може бути пусте";
            int imageId = UniqueNumberGenerator.GenerateInt();
            ImageExtracter.Extract(imageId);
            var partner = new CreatePartnerDto()
            {
                Title = string.Empty,
                Description = "Test",
                LogoId = imageId,
            };

            var response = await this.Client.CreateAsync(partner, this.tokenStorage.AdminAccessToken);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<ErrorDto>>(response.Content);

            Assert.True(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            Assert.NotNull(returnedValue);
            Assert.Contains(returnedValue, e => e.Message.Equals(expectedError));
            BaseExtracter.RemoveById<Image>(imageId);
        }

        [Fact]
        public async Task GetAll_ReturnSuccessStatusCode()
        {
            var response = await this.Client.GetAllAsync();
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<GetAllPartnersResponseDto>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetById_ReturnSuccessStatusCode()
        {
            Partner expectedPartner = this.testPartner;
            var response = await this.Client.GetByIdAsync(expectedPartner.Id);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<PartnerDto>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
            Assert.Multiple(
                () => Assert.Equal(expectedPartner.Id, returnedValue.Id),
                () => Assert.Equal(expectedPartner.Title, returnedValue.Title),
                () => Assert.Equal(expectedPartner.Description, returnedValue.Description),
                () => Assert.Equal(expectedPartner.LogoId, returnedValue.LogoId),
                () => Assert.Equal(expectedPartner.IsKeyPartner, returnedValue.IsKeyPartner),
                () => Assert.Equal(expectedPartner.TargetUrl, returnedValue.TargetUrl?.Href),
                () => Assert.Equal(expectedPartner.UrlTitle, returnedValue.TargetUrl?.Title));
        }

        [Fact]
        public async Task PartnersControllerTests_GetByIdIncorrectReturnBadRequest()
        {
            int id = -100;
            var response = await this.Client.GetByIdAsync(id);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetByStreetcodeId_ReturnSuccessStatusCode()
        {
            int streetcodeId = this.testStreetcodeContent.Id;

            var response = await this.Client.GetByStreetcodeId(streetcodeId);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<IEnumerable<PartnerDto>>(response.Content);

            Assert.True(response.IsSuccessStatusCode);
            Assert.NotNull(returnedValue);
        }

        [Fact]
        public async Task GetByStreetcodeId_Incorrect_ReturnBadRequest()
        {
            int streetcodeId = -100;
            var response = await this.Client.GetByStreetcodeId(streetcodeId);

            Assert.Multiple(
                          () => Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode),
                          () => Assert.False(response.IsSuccessStatusCode));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeContentExtracter.Remove(this.testStreetcodeContent);
                PartnerExtracter.Remove(this.testPartner);
            }

            base.Dispose(disposing);
        }
    }
}
