using Newtonsoft.Json;
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
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Xunit;

namespace Streetcode.XIntegrationTest.ControllerTests.Streetcode
{
    public class StreetcodeControllerTests :
        BaseControllerTests, IClassFixture<CustomWebApplicationFactory<Program>>
    {
        public StreetcodeControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory, "/api/Streetcode")
        {

        }

        [Fact]
        [ExtractTestStreetcode]
        public async Task Update_ReturnSuccessStatusCode()
        {
            StreetcodeContent expectedStreetcode = ExtractTestStreetcode.StreetcodeForTest;

            //  var updateStreetcodeCommand = new UpdateStreetcodeCommand(
            var updateStreetCodeDTO = new StreetcodeUpdateDTO
            {
                Id = expectedStreetcode.Id,
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
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>()
            };
            var response = await this.client.UpdateAsync(updateStreetCodeDTO);
            StreetcodeContent updatedStreetcode = ExtractTestStreetcode.StreetcodeForTest;

            var responseContent = JsonConvert.DeserializeObject<StreetcodeContent>(response.Content);

            Assert.Multiple(() =>
            {
                Assert.True(response.IsSuccessStatusCode);
                Assert.Equal(updatedStreetcode.Title, responseContent.Title);
                Assert.Equal(updatedStreetcode.TransliterationUrl, responseContent.TransliterationUrl);
            });
        }
    }
}
