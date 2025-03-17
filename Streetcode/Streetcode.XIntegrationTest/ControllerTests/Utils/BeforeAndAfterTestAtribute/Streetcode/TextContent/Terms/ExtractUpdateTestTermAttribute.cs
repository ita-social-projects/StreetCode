using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent.Term;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.Terms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateTestTermAttribute : BeforeAfterTestAttribute
{
    public static TermDto TermUpdateDtoForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        TermUpdateDtoForTest = new TermDto()
        {
            Id = 1,
            Title = "TermDto for update test",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var term = sqlDbHelper.GetExistItem<Term>(relatedTerm => relatedTerm.Title == TermUpdateDtoForTest.Title);

        if (term is not null)
        {
            sqlDbHelper.DeleteItem(term);
        }
    }
}
