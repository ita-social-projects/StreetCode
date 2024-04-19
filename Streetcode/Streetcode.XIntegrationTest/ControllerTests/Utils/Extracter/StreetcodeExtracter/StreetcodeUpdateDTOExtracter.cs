using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics.Update;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.DTO.Media.Create;
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

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter
{
    public class StreetcodeUpdateDTOExtracter
    {
        public static StreetcodeUpdateDTO Extract(int id, int index, string transliterationUrl)
        {
            StreetcodeContent testStreetcode = StreetcodeContentExtracter.Extract(id, index, transliterationUrl);

            return GetTestStreetcodeUpdateDTO(testStreetcode.Id, testStreetcode.Index, testStreetcode.TransliterationUrl);
        }

        public static void Remove(StreetcodeUpdateDTO entity)
        {
            BaseExtracter.RemoveByPredicate<StreetcodeContent>(strCont => strCont.Id == entity.Id);
        }

        private static StreetcodeUpdateDTO GetTestStreetcodeUpdateDTO(int id, int index, string transliterationUrl)
        {
            return new StreetcodeUpdateDTO
            {
                Id = id,
                Index = index,
                TransliterationUrl = transliterationUrl,
                Title = "Test_Title",
                DateString = "Test_Data",
                Tags = new List<StreetcodeTagUpdateDTO>(),
                Facts = new List<FactUpdateDto>(),
                Audios = new List<AudioUpdateDTO>(),
                Images = new List<ImageUpdateDTO>(),
                Videos = new List<VideoUpdateDTO>(),
                Partners = new List<PartnersUpdateDTO>(),
                Toponyms = new List<StreetcodeToponymUpdateDTO>(),
                Subtitles = new List<SubtitleUpdateDTO>(),
                TimelineItems = new List<TimelineItemCreateUpdateDTO>(),
                RelatedFigures = new List<RelatedFigureUpdateDTO>(),
                Arts = new List<ArtCreateUpdateDTO>(),
                StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordUpdateDTO>(),
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>(),
            };
        }
    }
}
