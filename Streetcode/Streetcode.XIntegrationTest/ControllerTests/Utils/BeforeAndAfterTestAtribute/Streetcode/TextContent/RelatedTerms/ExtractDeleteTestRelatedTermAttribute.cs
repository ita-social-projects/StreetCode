using System.Reflection;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.RelatedTerms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractDeleteTestRelatedTermAttribute : BeforeAfterTestAttribute
{
    public static RelatedTerm RelatedTermForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        const string relatedTermWord = "RelatedTerm for delete test";
        var relatedTermId = UniqueNumberGenerator.GenerateInt();

        RelatedTermForTest = RelatedTermExtracter.Extract(relatedTermId, relatedTermWord);
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var term = sqlDbHelper.GetExistItemId<Term>(RelatedTermForTest.TermId);

        if (term is not null)
        {
            sqlDbHelper.DeleteItem(term);
            sqlDbHelper.SaveChanges();
        }
    }
}
