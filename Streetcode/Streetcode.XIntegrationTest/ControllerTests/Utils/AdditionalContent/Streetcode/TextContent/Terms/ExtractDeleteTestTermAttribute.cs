using System.Reflection;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Terms;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractDeleteTestTermAttribute : BeforeAfterTestAttribute
{
    public static Term TermForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        const string termTitle = "Term for delete test";
        var termId = UniqueNumberGenerator.GenerateInt();

        TermForTest = TermExtracter.Extract(termId, termTitle);
    }
}
