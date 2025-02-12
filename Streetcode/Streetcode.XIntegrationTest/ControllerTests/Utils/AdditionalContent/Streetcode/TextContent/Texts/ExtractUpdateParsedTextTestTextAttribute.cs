using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Texts;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateParsedTextTestTextAttribute : BeforeAfterTestAttribute
{
    public static TextPreviewDTO TextPreviewUpdateDtoForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        TextPreviewUpdateDtoForTest = new TextPreviewDTO
        {
            TextContent = "Lorem ipsum dolor sit amet, consectetur adipiscing elit",
        };
    }
}
