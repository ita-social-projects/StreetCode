using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Art;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Image
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestImage: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Images.Image ImageForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            ImageForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Images.Image>();

            if (ImageForTest == null)
            {
                if (ExtractTestArt.ArtForTest == null)
                {
                    new ExtractTestArt().Before(null);
                }

                ImageForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Media.Images.Image()
                {
                    Base64 = "prettybase64",
                    BlobName = "blobName",
                    Art = ExtractTestArt.ArtForTest,
                });
                sqlDbHelper.SaveChanges();
            }
        }

    }
}
