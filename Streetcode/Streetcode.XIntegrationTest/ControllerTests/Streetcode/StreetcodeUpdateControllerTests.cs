using System.Net;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Enums;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Client.StreetCode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    [Collection("Authorization")]
    public class StreetcodeUpdateControllerTests :
        BaseAuthorizationControllerTests<StreetcodeClient>
    {
        private readonly StreetcodeUpdateDTO _testStreetcodeUpdateDto;

        public StreetcodeUpdateControllerTests(CustomWebApplicationFactory<Program> factory, TokenStorage tokenStorage)
            : base(factory, "/api/Streetcode", tokenStorage)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            int uniqueIndex = UniqueNumberGenerator.GenerateIntFromGuidInRange();
            _testStreetcodeUpdateDto = StreetcodeUpdateDTOExtracter.Extract(
                uniqueId,
                uniqueIndex,
                Guid.NewGuid().ToString());
        }

        [Fact]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeUpdateDTO updateStreetCodeDto = _testStreetcodeUpdateDto;

            var response = await Client.UpdateAsync(updateStreetCodeDto, TokenStorage.AdminAccessToken);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Update_ChangesTitleAndTransliterationUrl()
        {
            StreetcodeUpdateDTO updateStreetCodeDto = _testStreetcodeUpdateDto;
            await Client.UpdateAsync(updateStreetCodeDto, TokenStorage.AdminAccessToken);

            var responseGetByIdUpdated = await Client.GetByIdAsync(updateStreetCodeDto.Id);
            var streetCodeContent = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(responseGetByIdUpdated.Content);
            Assert.Multiple(() =>
            {
                Assert.Equal(updateStreetCodeDto.Title, streetCodeContent?.Title);
                Assert.Equal(updateStreetCodeDto.DateString, streetCodeContent?.DateString);
            });
        }

        [Fact]
        public async Task Update_WithInvalidId_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDto = new ()
            {
                Id = _testStreetcodeUpdateDto.Id + 1,
                Teaser = _testStreetcodeUpdateDto.Teaser,
                Title = _testStreetcodeUpdateDto.Title,
                DateString = _testStreetcodeUpdateDto.DateString,
            };

            var response = await Client.UpdateAsync(updateStreetCodeDto, TokenStorage.AdminAccessToken);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDto = _testStreetcodeUpdateDto;
            updateStreetCodeDto.Title = null!; // Invalid data

            var response = await Client.UpdateAsync(updateStreetCodeDto, TokenStorage.AdminAccessToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Update_WithInvalidTags_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDto = _testStreetcodeUpdateDto;

            // Invalid tag data
            updateStreetCodeDto.Tags = new List<StreetcodeTagUpdateDTO>
                    {
                        new ()
                        {
                            Id = 9999, // Non-existent tag ID
                            Title = "Invalid Tag",
                            IsVisible = true,
                            Index = 0,
                            StreetcodeId = updateStreetCodeDto.Id,
                            ModelState = ModelState.Updated,
                        },
                    };

            var response = await Client.UpdateAsync(updateStreetCodeDto, TokenStorage.AdminAccessToken);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchStage_UpdatesStatusSuccessfully()
        {
            // Arrange
            UpdateStatusStreetcodeByIdCommand request = new (_testStreetcodeUpdateDto.Id, StreetcodeStatus.Draft);

            // Act
            var response = await Client.PatchStageAsync(request, TokenStorage.AdminAccessToken);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);

            var updatedResponse = await Client.GetByIdAsync(request.Id);
            var updatedStreetcode = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeDTO>(updatedResponse.Content);
            Assert.NotNull(updatedStreetcode);
            Assert.Equal(request.Status, updatedStreetcode.Status);
        }

        [Fact]
        public async Task PatchStage_ReturnsUnauthorized_WhenNoAuthToken()
        {
            // Arrange
            UpdateStatusStreetcodeByIdCommand request = new (_testStreetcodeUpdateDto.Id, StreetcodeStatus.Published);

            // Act
            var response = await Client.PatchStageAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task PatchStage_ReturnsBadRequest_WhenStreetcodeNotExists()
        {
            // Arrange
            UpdateStatusStreetcodeByIdCommand request = new (-1999, StreetcodeStatus.Published);

            // Act
            var response = await Client.PatchStageAsync(request, TokenStorage.AdminAccessToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task PatchStage_ReturnsForbidden_WhenUserAccessToken()
        {
            // Arrange
            UpdateStatusStreetcodeByIdCommand request = new (_testStreetcodeUpdateDto.Id, StreetcodeStatus.Published);

            // Act
            var response = await Client.PatchStageAsync(request, TokenStorage.UserAccessToken);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                StreetcodeUpdateDTOExtracter.Remove(_testStreetcodeUpdateDto);
            }

            base.Dispose(disposing);
        }
    }
}