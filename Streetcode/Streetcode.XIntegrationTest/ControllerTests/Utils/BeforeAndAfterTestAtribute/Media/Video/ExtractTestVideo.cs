using Streetcode.DAL.Entities.Media;
using Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Video
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestVideo: BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Video VideoForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            VideoForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Video>();

            if (VideoForTest == null)
            {
                new ExtractTestStreetcode().Before(null);
                VideoForTest = sqlDbHelper.AddNewItem(new DAL.Entities.Media.Video()
                {
                    Description = "VideoDescription",
                    Title = "VideoTitle",
                    Url = "VideoUrl",
                    StreetcodeId = ExtractTestStreetcode.StreetcodeForTest.Id,
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
