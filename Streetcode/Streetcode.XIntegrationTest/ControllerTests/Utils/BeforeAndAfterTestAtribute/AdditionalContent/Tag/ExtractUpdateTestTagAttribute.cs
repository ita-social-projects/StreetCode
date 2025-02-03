using System.Reflection;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractUpdateTestTagAttribute : BeforeAfterTestAttribute
    {
        public static UpdateTagDto TagForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            TagForTest = new UpdateTagDto
            {
                Id = 1,
                Title = "New Title Test",
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var tag = sqlDbHelper.GetExistItem<DAL.Entities.AdditionalContent.Tag>(p => p.Title == TagForTest.Title);
            if (tag != null)
            {
                sqlDbHelper.DeleteItem(tag);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
