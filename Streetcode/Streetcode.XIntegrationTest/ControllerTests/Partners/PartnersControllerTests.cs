using System.Collections;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Resources;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Shared;
using Streetcode.BLL.Resources;
using Streetcode.BLL.SharedResource;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Partner;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Xunit.Abstractions;

using Resources = Streetcode.BLL;
namespace Streetcode.XIntegrationTest.ControllerTests.Partners
{
    [Collection("Authorization")]
    public class PartnersControllerTests : BaseControllerTests<PartnersClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeContent _testStreetcodeContent;
        private readonly TokenStorage _tokenStorage;
        private Partner _testPartner;

        public PartnersControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage, ITestOutputHelper output)
            : base(factory, "/api/Partners")
        {
            this._tokenStorage = tokenStorage;
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
        public async Task СreatePartner_ReturnBadRequest_WhenValidationFailed()
        {
            var expectedError = "Поле 'Назва' не може бути пусте";
            int imageId = UniqueNumberGenerator.GenerateInt();
            var image = ImageExtracter.Extract(imageId);
            var partner = new CreatePartnerDTO()
            {
                Title = string.Empty,
                Description = "Test",
                LogoId = imageId,
            };

            var response = await this.client.CreateAsync(partner, _tokenStorage.AdminAccessToken);
            var returnedValue = CaseIsensitiveJsonDeserializer.Deserialize<List<ErrorDto>>(response.Content);

            Assert.True(response.StatusCode.Equals(HttpStatusCode.BadRequest));
            Assert.NotNull(returnedValue);
            Assert.Contains(returnedValue, e => e.Message.Equals(expectedError));
            BaseExtracter.RemoveById<Image>(imageId);
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
