using Streetcode.DAL.Entities.Timeline;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Timelines
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestTimelineItem : BeforeAfterTestAttribute
    {
        public static TimelineItem TimelineItemForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            TimelineItemForTest = sqlDbHelper.GetExistItem<TimelineItem>();
            if (TimelineItemForTest == null)
            {
                TimelineItemForTest = sqlDbHelper.AddNewItem(new TimelineItem()
                {
                    Date = DateTime.Now,
                    Description = "TimelineItemForTest Description",
                    Title = "Title TimelineItemForTest"
                });
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
