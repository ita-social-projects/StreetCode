using System.Reflection;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Analytics;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExtractStatisticRecordAttribute : BeforeAfterTestAttribute
{
    public static int QrId { get; private set; }

    public static StreetcodeContent TestStreetcode { get; private set; } = null!;

    private static StreetcodeCoordinate TestCoordinate { get; set; } = null!;

    private static StatisticRecord TestStatisticRecord { get; set; } = null!;

    public override void Before(
        MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        QrId = UniqueNumberGenerator.GenerateInt();
        int streetcodeId = UniqueNumberGenerator.GenerateInt();

        TestStreetcode = StreetcodeContentExtracter
            .Extract(
                streetcodeId,
                streetcodeId,
                Guid.NewGuid().ToString());

        TestCoordinate = CoordinateExtracter.Extract(streetcodeId, streetcodeId);
        TestStatisticRecord = new StatisticRecord
        {
            QrId = QrId,
            Count = 10,
            Address = "Test Address",
            StreetcodeId = TestStreetcode.Id,
            StreetcodeCoordinateId = TestCoordinate.Id,
        };

        sqlDbHelper.AddNewItem(TestStatisticRecord);
        sqlDbHelper.SaveChanges();
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        sqlDbHelper.DeleteItem(TestStatisticRecord);
        sqlDbHelper.DeleteItem(TestCoordinate);
        sqlDbHelper.DeleteItem(TestStreetcode);
        sqlDbHelper.SaveChanges();
    }
}