using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.DTO.Timeline.Update;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Enums;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using System.Reflection;
using Xunit.Sdk;
using static Streetcode.XIntegrationTest.Constants.ControllerTests.StreetcodeConstants;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    class ExtractCreateTestStreetcode : BeforeAfterTestAttribute
    {
        public static StreetcodeCreateDTO StreetcodeForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            StreetcodeForTest = new StreetcodeCreateDTO
            {
                Index = STREETCODE_CREATE_INDEX,
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Title = "TestTitle",
                DateString = "20 травня 2023",
                Alias = "TestAlias",
                TransliterationUrl = Guid.NewGuid().ToString(),
                ARBlockURL = "test-arblock-url",
                StreetcodeType = StreetcodeType.Event,
                Status = StreetcodeStatus.Published,
                EventStartOrPersonBirthDate = DateTime.Now,
                EventEndOrPersonDeathDate = DateTime.Now.AddDays(1),
                ViewCount = 1,
                Teaser = "Test Teaser",
                Text = new TextCreateDTO
                {
                    Title = "TestTextTitle",
                    TextContent = "TestTextContent",
                    AdditionalText = "TestAdditionalText",
                },
                Toponyms = new List<StreetcodeToponymUpdateDTO>(),
                ImagesIds = new List<int>(),
                Tags = new List<StreetcodeTagDTO>(),
                Subtitles = new List<SubtitleCreateDTO>(),
                Facts = new List<FactUpdateCreateDto>(),
                Videos = new List<VideoCreateDTO>(),
                Partners = new List<PartnerShortDTO>(),
                Arts = new List<ArtCreateUpdateDTO>(),
                StreetcodeArtSlides = new List<StreetcodeArtSlideCreateUpdateDTO>(),
                StatisticRecords = new List<StatisticRecordDTO>(),
                StreetcodeCategoryContents = new List<CategoryContentCreateDTO>(),
                Coordinates = new List<StreetcodeCoordinateDTO>(),
                ImagesDetails = new List<ImageDetailsDto>(),
                TimelineItems = new List<TimelineItemCreateUpdateDTO>(),
                RelatedFigures = new List<RelatedFigureShortDTO>(),
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var streetcodeContent = sqlDbHelper.GetExistItem<StreetcodeContent>(p => p.Index == StreetcodeForTest.Index);
            if (streetcodeContent != null)
            {
                sqlDbHelper.DeleteItem(streetcodeContent);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
