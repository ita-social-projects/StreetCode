using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using System.Net;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeUpdateControllerTests :
        BaseAuthorizationControllerTests<StreetcodeClient>, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private StreetcodeUpdateDTO _testStreetcodeUpdateDTO;

        public StreetcodeUpdateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Streetcode", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.Generate();
            _testStreetcodeUpdateDTO = StreetcodeUpdateDTOExtracter.Extract(
                uniqueId,
                uniqueId,
                Guid.NewGuid().ToString());
        }

        public override void Dispose()
        {
            StreetcodeUpdateDTOExtracter.Remove(_testStreetcodeUpdateDTO);
        }

        [Fact]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = _testStreetcodeUpdateDTO;

            var response = await client.UpdateAsync(updateStreetCodeDTO, _tokenStorage.AdminToken);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Update_ChangesTitleAndTransliterationUrl()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = _testStreetcodeUpdateDTO;
            await client.UpdateAsync(updateStreetCodeDTO, _tokenStorage.AdminToken);

            var responseGetByIdUpdated = await client.GetByIdAsync(updateStreetCodeDTO.Id);
            var streetCodeContent = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(responseGetByIdUpdated.Content);
            Assert.Multiple(() =>
            {
                Assert.Equal(updateStreetCodeDTO.Title, streetCodeContent.Title);
                Assert.Equal(updateStreetCodeDTO.DateString, streetCodeContent.DateString);
            });
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = _testStreetcodeUpdateDTO;
            updateStreetCodeDTO.Id++;

            var response = await client.UpdateAsync(updateStreetCodeDTO, _tokenStorage.AdminToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = _testStreetcodeUpdateDTO;
            updateStreetCodeDTO.Title = null; // Invalid data

            var response = await client.UpdateAsync(updateStreetCodeDTO, _tokenStorage.AdminToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidTags_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = _testStreetcodeUpdateDTO;

            // Invalid tag data
            updateStreetCodeDTO.Tags = new List<StreetcodeTagUpdateDTO>
                    {
                        new StreetcodeTagUpdateDTO
                        {
                            Id = 9999, // Non-existent tag ID
                            Title = "Invalid Tag",
                            IsVisible = true,
                            Index = 0,
                            StreetcodeId = updateStreetCodeDTO.Id,
                            ModelState = ModelState.Updated
                        },
                    };

            var response = await client.UpdateAsync(updateStreetCodeDTO, _tokenStorage.AdminToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}