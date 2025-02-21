using System.Reflection;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.RelatedFigure;
using Xunit.Sdk;
using RelateFigureEntity = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.RelatedFigure;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractDeleteTestRelatedFigureAttribute : BeforeAfterTestAttribute
{
    public static StreetcodeContent StreetcodeContent1 { get; private set; } = new ();

    public static StreetcodeContent StreetcodeContent2 { get; private set; } = new ();

    public static RelateFigureEntity RelatedFigureForTest { get; private set; } = new ();

    public override void Before(MethodInfo methodUnderTest)
    {
        var observerId = UniqueNumberGenerator.GenerateInt();
        var targetId = UniqueNumberGenerator.GenerateInt();
        StreetcodeContent1 = StreetcodeContentExtracter
            .Extract(
                observerId,
                observerId,
                Guid.NewGuid().ToString());
        StreetcodeContent2 = StreetcodeContentExtracter
            .Extract(
                targetId,
                targetId,
                Guid.NewGuid().ToString());
        RelatedFigureForTest = RelatedFigureExtracter.Extract(StreetcodeContent1.Id, StreetcodeContent2.Id);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var existingStreetcode1 = sqlDbHelper.GetExistItem<StreetcodeContent>(sc => sc.Id == StreetcodeContent1.Id);
        var existingStreetcode2 = sqlDbHelper.GetExistItem<StreetcodeContent>(sc => sc.Id == StreetcodeContent2.Id);
        if (existingStreetcode2 != null)
        {
            sqlDbHelper.DeleteItem(existingStreetcode2);
            sqlDbHelper.SaveChanges();
        }

        if (existingStreetcode1 != null)
        {
            sqlDbHelper.DeleteItem(existingStreetcode1);
            sqlDbHelper.SaveChanges();
        }
    }
}