using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Facts;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractCreateTestFactAttribute : BeforeAfterTestAttribute
{
    public static StreetcodeFactCreateDTO FactCreateDtoForTest { get; private set; } = null!;

    private static Image ImageForTest { get; set; } = null!;

    private static StreetcodeContent StreetcodeForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var factIndex = UniqueNumberGenerator.GenerateInt();
        var imageId = UniqueNumberGenerator.GenerateInt();
        var streetcodeContentId = UniqueNumberGenerator.GenerateInt();

        ImageForTest = ImageExtracter.Extract(imageId);
        StreetcodeForTest = StreetcodeContentExtracter.Extract(
            streetcodeContentId,
            streetcodeContentId,
            Guid.NewGuid().ToString());
        FactCreateDtoForTest = new StreetcodeFactCreateDTO()
        {
            Title = "StreetcodeFactCreateDto for create test",
            ImageId = ImageForTest.Id,
            FactContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            StreetcodeId = StreetcodeForTest.Id,
            Index = factIndex,
            ImageDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var fact = sqlDbHelper.GetExistItem<Fact>(fact => fact.Title == FactCreateDtoForTest.Title);

        if (fact is not null)
        {
            sqlDbHelper.DeleteItem(fact);
        }

        ImageExtracter.Remove(ImageForTest);
        StreetcodeContentExtracter.Remove(StreetcodeForTest);
    }
}
