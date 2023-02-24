using Streetcode.DAL.Entities.Media;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Audio
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestAudio: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Audio AudioForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            AudioForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Audio>();
            if (AudioForTest == null)
            {
                if (ExtractTestStreetcode.StreetcodeForTest == null)
                {
                    new ExtractTestStreetcode().Before(null);
                }

                sqlDbHelper.SaveChanges();
                sqlDbHelper.AddNewItem(new DAL.Entities.Media.Audio()
                {
                    Url = "imageUrl",
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
