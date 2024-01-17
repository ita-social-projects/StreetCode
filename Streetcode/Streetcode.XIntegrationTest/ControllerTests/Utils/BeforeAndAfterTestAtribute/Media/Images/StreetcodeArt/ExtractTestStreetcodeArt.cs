using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.Art;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Images.StreetcodeArt
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestStreetcodeArt : BeforeAfterTestAttribute
    {
        public static DAL.Entities.Streetcode.StreetcodeArt StreetcodeArtForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            StreetcodeArtForTest = sqlDbHelper.GetExistItem<DAL.Entities.Streetcode.StreetcodeArt>();
            if (StreetcodeArtForTest == null)
            {
                if (ExtractTestStreetcode.StreetcodeForTest == null)
                {
                    new ExtractTestStreetcode().Before(null);
                }

                if (ExtractTestArt.ArtForTest == null)
                {
                    new ExtractTestArt().Before(null);
                }

                StreetcodeArtForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Streetcode.StreetcodeArt()
                {
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id,
                    ArtId = ExtractTestArt.ArtForTest.Id,
                    Art = ExtractTestArt.ArtForTest,
                    Streetcode = ExtractTestStreetcode.StreetcodeForTest,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
