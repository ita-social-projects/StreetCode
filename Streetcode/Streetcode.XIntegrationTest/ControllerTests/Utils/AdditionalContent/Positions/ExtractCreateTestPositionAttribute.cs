using System.Reflection;
using Streetcode.BLL.DTO.Team;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Positions;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExtractCreateTestPositionAttribute : BeforeAfterTestAttribute
{
    public static PositionDTO PositionForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        PositionForTest = new PositionDTO
        {
            Position = "Test",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var positions = sqlDbHelper.GetExistItem<DAL.Entities.Team.Positions>(p => p.Position == PositionForTest.Position);
        if (positions != null)
        {
            sqlDbHelper.DeleteItem(positions);
            sqlDbHelper.SaveChanges();
        }
    }
}
