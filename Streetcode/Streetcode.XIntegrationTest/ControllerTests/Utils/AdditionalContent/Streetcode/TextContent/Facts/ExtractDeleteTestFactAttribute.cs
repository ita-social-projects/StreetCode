using System.Reflection;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Facts;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractDeleteTestFactAttribute : BeforeAfterTestAttribute
{
    public static Fact FactForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        const string factTitle = "Fact for delete test";
        var factId = UniqueNumberGenerator.GenerateInt();

        FactForTest = FactExtracter.Extract(factId, factTitle);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var streetcodeContent = sqlDbHelper.GetExistItemId<StreetcodeContent>(FactForTest.StreetcodeId);
        var image = sqlDbHelper.GetExistItemId<Image>((int)FactForTest.ImageId!);

        if (streetcodeContent is not null)
        {
            sqlDbHelper.DeleteItem(streetcodeContent);
            sqlDbHelper.SaveChanges();
        }

        if (image is not null)
        {
            sqlDbHelper.DeleteItem(image);
            sqlDbHelper.SaveChanges();
        }
    }
}
