using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Video
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestVideo: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Video VideoForTest;
        public static StreetcodeContent StreetcodeWithVideo;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            VideoForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Video>();
            if (VideoForTest == null)
            {
                new ExtractTestStreetcode().Before(methodUnderTest);
                VideoForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Media.Video()
                {
                    Description = "VideoDescription",
                    Title = "VideoTitle",
                    Url = "VideoUrl",
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id,
                });
                sqlDbHelper.SaveChanges();
            }

            StreetcodeWithVideo = sqlDbHelper.GetExistItem<StreetcodeContent>(x => x.Id == VideoForTest.StreetcodeId);

        }
    }
}
