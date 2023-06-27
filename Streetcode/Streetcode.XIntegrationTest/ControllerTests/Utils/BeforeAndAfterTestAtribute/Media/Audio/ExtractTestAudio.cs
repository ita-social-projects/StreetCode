using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Audio
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestAudio : BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Audio? AudioForTest;

        /// <inheritdoc/>
        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            AudioForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Audio>();
            if (AudioForTest == null)
            {
                if (ExtractTestStreetcode.StreetcodeForTest == null)
                {
                    new ExtractTestStreetcode().Before(methodUnderTest);
                }

                sqlDbHelper.SaveChanges();
                sqlDbHelper.AddNewItem(new DAL.Entities.Media.Audio()
                {
                    BlobName = "nicebase64",
                    Title = "Title",
                    MimeType = "mpeg",
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
