using System.Reflection;
using Streetcode.BLL.DTO.Streetcode.TextContent.Fact;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.AdditionalContent.Streetcode.TextContent.Facts;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateTestFactAttribute : BeforeAfterTestAttribute
{
    public static FactDto FactUpdateDtoForTest { get; private set; } = null!;

    private static Image ImageForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var factIndex = UniqueNumberGenerator.GenerateInt();
        var imageId = UniqueNumberGenerator.GenerateInt();

        ImageForTest = ImageExtracter.Extract(imageId);
        FactUpdateDtoForTest = new FactDto
        {
            Id = 1,
            Title = "FactDto for update test",
            ImageId = ImageForTest.Id,
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

        ImageExtracter.Remove(ImageForTest);
    }
}
