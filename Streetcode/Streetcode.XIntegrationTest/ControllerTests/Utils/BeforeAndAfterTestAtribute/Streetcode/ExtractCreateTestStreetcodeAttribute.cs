using System.Reflection;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.DTO.Analytics;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.DTO.Media.Create;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.DTO.Media.Video;
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
using Xunit.Sdk;
using static Streetcode.XIntegrationTest.Constants.ControllerTests.StreetcodeConstants;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractCreateTestStreetcodeAttribute : BeforeAfterTestAttribute
{
    public static StreetcodeCreateDTO StreetcodeForTest { get; set; } = null!;

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
            ArBlockUrl = "test-arblock-url",
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
            Toponyms = new List<StreetcodeToponymCreateUpdateDTO>(),
            Tags = new List<StreetcodeTagDTO>(),
            Subtitles = new List<SubtitleCreateDTO>(),
            Facts = new List<StreetcodeFactCreateDTO>(),
            Videos = new List<VideoCreateDTO>(),
            Partners = new List<int>(),
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