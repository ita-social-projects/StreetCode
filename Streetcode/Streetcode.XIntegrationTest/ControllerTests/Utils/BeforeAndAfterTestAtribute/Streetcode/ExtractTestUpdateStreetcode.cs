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
using System.Reflection;
using Streetcode.BLL.DTO.Media.Create;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class ExtractTestUpdateStreetcode : BeforeAfterTestAttribute
    {
        public static StreetcodeUpdateDTO StreetcodeForTest;
        private static StreetcodeContent _streetcodeContent;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            _streetcodeContent = sqlDbHelper.GetExistItem<StreetcodeContent>();
            if (_streetcodeContent == null)
            {
                _streetcodeContent = sqlDbHelper.AddNewItem(new StreetcodeContent()
                {
                    Index = new Random().Next(0, 1000000),
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    EventStartOrPersonBirthDate = DateTime.Now,
                    EventEndOrPersonDeathDate = DateTime.Now,
                    ViewCount = 1,
                    DateString = "20 травня 2023",
                    Alias = "dsf",
                    Title = "Title",
                    TransliterationUrl = Guid.NewGuid().ToString(),
                    Teaser = "Test Teaser",
                });
                sqlDbHelper.SaveChanges();
            }

            StreetcodeForTest = this.CreateMoqStreetCodeDTO(
                _streetcodeContent.Id,
                _streetcodeContent.Index,
                _streetcodeContent.TransliterationUrl);
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var streetcodeContent = sqlDbHelper.GetExistItem<StreetcodeContent>();
            if (streetcodeContent != null)
            {
                // Restore the original StreetcodeContent
                streetcodeContent.EventStartOrPersonBirthDate = _streetcodeContent.EventStartOrPersonBirthDate;
                streetcodeContent.EventEndOrPersonDeathDate = _streetcodeContent.EventEndOrPersonDeathDate;
                streetcodeContent.ViewCount = _streetcodeContent.ViewCount;
                streetcodeContent.DateString = _streetcodeContent.DateString;
                streetcodeContent.Alias = _streetcodeContent.Alias;
                streetcodeContent.Title = _streetcodeContent.Title;
                streetcodeContent.TransliterationUrl = _streetcodeContent.TransliterationUrl;
                streetcodeContent.Teaser = _streetcodeContent.Teaser;

                sqlDbHelper.SaveChanges();
            }
        }

        private StreetcodeUpdateDTO CreateMoqStreetCodeDTO(int id, int index, string transliterationUrl)
        {
            return new StreetcodeUpdateDTO
            {
                Id = id,
                Index = index,
                Title = "New Title",
                TransliterationUrl = transliterationUrl,
                Tags = new List<StreetcodeTagUpdateDTO>(),
                Facts = new List<FactUpdateDto>(),
                Audios = new List<AudioUpdateDTO>(),
                Images = new List<ImageUpdateDTO>(),
                Videos = new List<VideoUpdateDTO>(),
                Partners = new List<PartnersUpdateDTO>(),
                Toponyms = new List<StreetcodeToponymUpdateDTO>(),
                Subtitles = new List<SubtitleUpdateDTO>(),
                DateString = "22 травня 2023",
                TimelineItems = new List<TimelineItemCreateUpdateDTO>(),
                RelatedFigures = new List<RelatedFigureUpdateDTO>(),
                StreetcodeArts = new List<StreetcodeArtCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordUpdateDTO>(),
                StreetcodeCategoryContents = new List<StreetcodeCategoryContentUpdateDTO>(),
            };
        }
    }
}