using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners.Update;
using Streetcode.BLL.DTO.Sources.Update;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.Update;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
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
        [ExtractTestStreetcode]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;

            var updateStreetCodeDTO = CreateMoqStreetCodeDTO(expectedStreetcode.Id);
            var response = await client.UpdateAsync(updateStreetCodeDTO);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_ChangesTitleAndTransliterationUrl()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;

            var updateStreetCodeDTO = CreateMoqStreetCodeDTO(expectedStreetcode.Id);
            await client.UpdateAsync(updateStreetCodeDTO);

            var responseGetByIdUpdated = await client.GetByIdAsync(expectedStreetcode.Id);
            var streetCodeContent = CaseIsensitiveJsonDeserializer.Deserialize<StreetcodeContent>(responseGetByIdUpdated.Content);
            Assert.Multiple(() =>
            {
                Assert.Equal(updateStreetCodeDTO.Title, streetCodeContent.Title);
                Assert.Equal(updateStreetCodeDTO.TransliterationUrl, streetCodeContent.TransliterationUrl);
            });
        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_WithIncorrectId_ReturnsBadRequest()
        {

            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;
            var updateStreetCodeDTO = CreateMoqStreetCodeDTO(expectedStreetcode.Id + 1);
            var response = await client.UpdateAsync(updateStreetCodeDTO);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_WithInvalidData_ReturnsBadRequest()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;
            var updateStreetCodeDTO = CreateMoqStreetCodeDTO(expectedStreetcode.Id);
            updateStreetCodeDTO.Title = null; // Invalid data
            var response = await client.UpdateAsync(updateStreetCodeDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_WithInvalidTags_ReturnsBadRequest()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;
            var updateStreetCodeDTO = CreateMoqStreetCodeDTO(expectedStreetcode.Id);

            // Invalid tag data
            updateStreetCodeDTO.Tags = new List<StreetcodeTagUpdateDTO>
                    {
                        new StreetcodeTagUpdateDTO
                        {
                            Id = 9999, // Non-existent tag ID
                            Title = "Invalid Tag",
                            IsVisible = true,
                            Index = 0,
                            StreetcodeId = expectedStreetcode.Id,
                            ModelState = ModelState.Updated
                        },
                    };

            var response = await client.UpdateAsync(updateStreetCodeDTO);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private StreetcodeUpdateDTO CreateMoqStreetCodeDTO(int id)
        {
            return new StreetcodeUpdateDTO
            {
                Id = id,
                Title = "New Title",
                TransliterationUrl = "new-transliteration-url",
                Tags = new List<StreetcodeTagUpdateDTO>(),
                Facts = new List<FactUpdateDto>(),
                Audios = new List<AudioUpdateDTO>(),
                Images = new List<ImageUpdateDTO>(),
                Videos = new List<VideoUpdateDTO>(),
                Partners = new List<PartnersUpdateDTO>(),
                Toponyms = new List<StreetcodeToponymUpdateDTO>(),
                Subtitles = new List<SubtitleUpdateDTO>(),
                DateString = "20 травня 2023",
                TimelineItems = new List<TimelineItemCreateUpdateDTO>(),
                RelatedFigures = new List<RelatedFigureUpdateDTO>(),
                StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordUpdateDTO>(),
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>(),
            };
        }
    }
}
