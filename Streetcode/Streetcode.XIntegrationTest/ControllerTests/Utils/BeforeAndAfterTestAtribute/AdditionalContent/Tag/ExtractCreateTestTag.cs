using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractCreateTestTag : BeforeAfterTestAttribute
    {
        public static CreateTagDTO TagForTest;

        public override void Before(MethodInfo methodUnderTest)
        {
            TagForTest = new CreateTagDTO
            {
                Title = "Test",
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
