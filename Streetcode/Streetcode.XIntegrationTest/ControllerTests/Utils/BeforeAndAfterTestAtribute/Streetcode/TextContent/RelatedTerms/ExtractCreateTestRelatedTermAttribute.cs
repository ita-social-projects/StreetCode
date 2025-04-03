using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode.TextContent.RelatedTerms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractCreateTestRelatedTermAttribute : BeforeAfterTestAttribute
{
    public static RelatedTermCreateDTO RelatedTermCreateDtoForTest { get; private set; } = null!;

    private static Term TermForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var termId = UniqueNumberGenerator.GenerateInt();

        TermForTest = TermExtracter.Extract(termId);
        RelatedTermCreateDtoForTest = new RelatedTermCreateDTO()
        {
            Word = "RelatedTermCreateDto for create test",
            TermId = TermForTest.Id,
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var relatedTerm = sqlDbHelper.GetExistItem<RelatedTerm>(relatedTerm => relatedTerm.Word == RelatedTermCreateDtoForTest.Word);

        if (relatedTerm is not null)
        {
            sqlDbHelper.DeleteItem(relatedTerm);
        }

        TermExtracter.Remove(TermForTest);
    }
}
