using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.Enums;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Net;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode.Update
{
    public class StreetcodeUpdateControllerTests :
        BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeUpdateControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Streetcode")
        {

        }

        [Fact]
        [ExtractTestUpdateStreetcode]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = ExtractTestUpdateStreetcode.StreetcodeForTest;

            var response = await client.UpdateAsync(updateStreetCodeDTO);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        [ExtractTestUpdateStreetcode]
        public async Task Update_ChangesTitleAndTransliterationUrl()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = ExtractTestUpdateStreetcode.StreetcodeForTest;
            await client.UpdateAsync(updateStreetCodeDTO);

            var responseGetByIdUpdated = await client.GetByIdAsync(updateStreetCodeDTO.Id);
            var streetCodeContent = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(responseGetByIdUpdated.Content);
            Assert.Multiple(() =>
            {
                Assert.Equal(updateStreetCodeDTO.Title, streetCodeContent.Title);
                Assert.Equal(updateStreetCodeDTO.DateString, streetCodeContent.DateString);
            });
        }

        [Fact]
        [ExtractTestUpdateStreetcode]
        public async Task Update_WithInvalidId_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = ExtractTestUpdateStreetcode.StreetcodeForTest;
            updateStreetCodeDTO.Id++;

            var response = await client.UpdateAsync(updateStreetCodeDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestUpdateStreetcode]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = ExtractTestUpdateStreetcode.StreetcodeForTest;
            updateStreetCodeDTO.Title = null; // Invalid data

            var response = await client.UpdateAsync(updateStreetCodeDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestUpdateStreetcode]
        public async Task Update_WithInvalidTags_ReturnsBadRequest()
        {
            StreetcodeUpdateDTO updateStreetCodeDTO = ExtractTestUpdateStreetcode.StreetcodeForTest;

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

            var response = await client.UpdateAsync(updateStreetCodeDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}