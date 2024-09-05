using System.Reflection;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.DAL.Entities.Timeline;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Timeline;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExtractCreateTestHistoricalContext: BeforeAfterTestAttribute
{
    public static HistoricalContextDTO HistoricalContextForTest;

    public override void Before(MethodInfo methodUnderTest)
    {
        HistoricalContextForTest = new HistoricalContextDTO
        {
            Title = "Test",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var context = sqlDbHelper.GetExistItem<HistoricalContext>(p => p.Title == HistoricalContextForTest.Title);
        if (context != null)
        {
            sqlDbHelper.DeleteItem(context);
            sqlDbHelper.SaveChanges();
        }
    }
}