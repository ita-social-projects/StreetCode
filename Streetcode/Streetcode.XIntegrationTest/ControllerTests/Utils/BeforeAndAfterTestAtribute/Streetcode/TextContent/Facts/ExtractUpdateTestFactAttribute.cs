using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Facts;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateTestFactAttribute : BeforeAfterTestAttribute
{
    public static StreetcodeFactUpdateDTO FactUpdateDtoForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var factIndex = UniqueNumberGenerator.GenerateInt();

        FactUpdateDtoForTest = new StreetcodeFactUpdateDTO()
        {
            Id = 1,
            Title = "FactDto for update test",
            FactContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
            Index = factIndex,
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var fact = sqlDbHelper.GetExistItem<Fact>(fact => fact.Title == FactUpdateDtoForTest.Title);

        if (fact is not null)
        {
            sqlDbHelper.DeleteItem(fact);
        }
    }
}
