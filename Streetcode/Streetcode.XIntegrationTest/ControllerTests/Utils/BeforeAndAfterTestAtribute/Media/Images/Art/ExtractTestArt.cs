using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Image;
using System.Reflection;
using System.Runtime.InteropServices;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Art
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestArt: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Images.Art ArtForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            ArtForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Images.Art>();
            if (ArtForTest == null)
            {
                ExtractTestImage extractImageTest = new ExtractTestImage();
                extractImageTest.Before(null);

                ArtForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Media.Images.Art()
                {
                    Description = "ArtDescription",
                    ImageId = ExtractTestImage.ImageForTest.Id,
                });
                sqlDbHelper.SaveChanges();

            }
        }
    }
}
