using System.Reflection;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.RelatedFigure;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.RelatedFigure;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExtractCreateTestRelatedFigureAttribute : BeforeAfterTestAttribute
{
    public static StreetcodeContent StreetcodeContent1 { get; private set; } = null!;

    public static StreetcodeContent StreetcodeContent2 { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        int observerId = UniqueNumberGenerator.GenerateInt();
        int targetId = UniqueNumberGenerator.GenerateInt();
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
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var relatedFigures = sqlDbHelper.GetAll<DAL.Entities.Streetcode.RelatedFigure>(
            rf => rf.ObserverId == StreetcodeContent1.Id || rf.TargetId == StreetcodeContent2.Id);
        foreach (var relatedFigure in relatedFigures)
        {
            RelatedFigureExtracter.Remove(relatedFigure);
        }

        StreetcodeContentExtracter.Remove(StreetcodeContent1);
        StreetcodeContentExtracter.Remove(StreetcodeContent2);
    }
}