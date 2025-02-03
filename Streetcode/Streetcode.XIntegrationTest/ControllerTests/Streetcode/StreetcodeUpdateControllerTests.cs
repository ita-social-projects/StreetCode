using System.Net;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeUpdateControllerTests :
        BaseAuthorizationControllerTests<StreetcodeClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly StreetcodeUpdateDto testStreetcodeUpdateDTO;

        public StreetcodeUpdateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Streetcode", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            int uniqueIndex = UniqueNumberGenerator.GenerateIntFromGuidInRange();
            this.testStreetcodeUpdateDTO = StreetcodeUpdateDtoExtracter.Extract(
                uniqueId,
                uniqueIndex,
                Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeUpdateDto updateStreetCodeDTO = this.testStreetcodeUpdateDTO;

            var response = await this.Client.UpdateAsync(updateStreetCodeDTO, this.TokenStorage.AdminAccessToken);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Update_ChangesTitleAndTransliterationUrl()
        {
            StreetcodeUpdateDto updateStreetCodeDTO = this.testStreetcodeUpdateDTO;
            await this.Client.UpdateAsync(updateStreetCodeDTO, this.TokenStorage.AdminAccessToken);

            var responseGetByIdUpdated = await this.Client.GetByIdAsync(updateStreetCodeDTO.Id);
            var streetCodeContent = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(responseGetByIdUpdated.Content);
            Assert.Multiple(() =>
            {
                Assert.Equal(updateStreetCodeDTO.Title, streetCodeContent?.Title);
                Assert.Equal(updateStreetCodeDTO.DateString, streetCodeContent?.DateString);
            });
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsBadRequest()
        {
            StreetcodeUpdateDto updateStreetCodeDTO = this.testStreetcodeUpdateDTO;
            updateStreetCodeDTO.Id++;

            var response = await this.Client.UpdateAsync(updateStreetCodeDTO, this.TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            StreetcodeUpdateDto updateStreetCodeDTO = this.testStreetcodeUpdateDTO;
            updateStreetCodeDTO.Title = null!; // Invalid data

            var response = await this.Client.UpdateAsync(updateStreetCodeDTO, this.TokenStorage.AdminAccessToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidTags_ReturnsBadRequest()
        {
            StreetcodeUpdateDto updateStreetCodeDTO = this.testStreetcodeUpdateDTO;

            // Invalid tag data
            updateStreetCodeDTO.Tags = new List<StreetcodeTagUpdateDto>
                    {
                        new StreetcodeTagUpdateDto
                        {
                            Id = 9999, // Non-existent tag ID
                            Title = "Invalid Tag",
                            IsVisible = true,
                            Index = 0,
                            StreetcodeId = updateStreetCodeDTO.Id,
                            ModelState = ModelState.Updated,
                        },
                    };

            var response = await this.Client.UpdateAsync(updateStreetCodeDTO, this.TokenStorage.AdminAccessToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeUpdateDtoExtracter.Remove(this.testStreetcodeUpdateDTO);
            }

            base.Dispose(disposing);
        }
    }
}