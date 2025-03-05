using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.RelatedTerms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateTestRelatedTermAttribute : BeforeAfterTestAttribute
{
    public static RelatedTermDTO RelatedTermUpdateDtoForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        RelatedTermUpdateDtoForTest = new RelatedTermDTO()
        {
            Id = 1,
            Word = "RelatedTermDto for update test",
            TermId = 1,
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var relatedTerm = sqlDbHelper.GetExistItem<RelatedTerm>(relatedTerm => relatedTerm.Word == RelatedTermUpdateDtoForTest.Word);

        if (relatedTerm is not null)
        {
            sqlDbHelper.DeleteItem(relatedTerm);
        }
    }
}
