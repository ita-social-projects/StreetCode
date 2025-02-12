using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Terms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractCreateTestTermAttribute : BeforeAfterTestAttribute
{
    public static TermCreateDTO TermCreateDtoForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        TermCreateDtoForTest = new TermCreateDTO
        {
            Title = "TermCreateDto for create test",
            Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var term = sqlDbHelper.GetExistItem<Term>(term => term.Title == TermCreateDtoForTest.Title);

        if (term is not null)
        {
            sqlDbHelper.DeleteItem(term);
        }
    }
}
