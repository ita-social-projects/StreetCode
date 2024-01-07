using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Image
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractTestImage: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Images.Image ImageForTest;

        public override async void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            ImageForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Images.Image>();

            if (ImageForTest == null)
            {
                var blobFixture = new BlobStorageFixture();
                ImageForTest = blobFixture.SeedImage(Guid.NewGuid().ToString());
                ImageForTest.BlobName += $".{ImageForTest.MimeType?.Split('/')[1]}";
                sqlDbHelper.AddNewItem(ImageForTest);
                sqlDbHelper.SaveChanges();
                new ExtractTestStreetcode().Before(methodUnderTest);
                StreetcodeImage streetcodeImage = new StreetcodeImage()
                {
                    ImageId = ImageForTest.Id,
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id,
                };
            }
        }

    }
}
