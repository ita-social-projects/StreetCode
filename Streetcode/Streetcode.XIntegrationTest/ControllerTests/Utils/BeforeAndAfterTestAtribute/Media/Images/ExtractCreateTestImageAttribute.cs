using System.Reflection;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images;

public class ExtractCreateTestImageAttribute : BeforeAfterTestAttribute
{
    public static ImageDTO ImageForTest { get; set; } = null!;

    public static ImageFileBaseCreateDTO ImageFileCreateForTest { get; set; } = null!;

    public static ImageFileBaseUpdateDTO ImageFileUpdateForTest { get; set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        ImageForTest = new ImageDTO
        {
            BlobName = "TestName",
            MimeType = "image/png",
        };

        ImageFileCreateForTest = new ImageFileBaseCreateDTO
        {
            Title = "TestName",
            BaseFormat = "TestBase",
            MimeType = "image/png",
            Extension = "png",
            Alt = "0",
        };

        ImageFileUpdateForTest = new ImageFileBaseUpdateDTO
        {
            Title = "UpdatedName",
            BaseFormat = "TestBase",
            MimeType = "image/png",
            Extension = "png",
            Alt = "0",
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var image = sqlDbHelper.GetExistItem<Image>(p => p.Id == ImageForTest.Id);
        if (image != null)
        {
            sqlDbHelper.DeleteItem(image);
            sqlDbHelper.SaveChanges();
        }
    }
}