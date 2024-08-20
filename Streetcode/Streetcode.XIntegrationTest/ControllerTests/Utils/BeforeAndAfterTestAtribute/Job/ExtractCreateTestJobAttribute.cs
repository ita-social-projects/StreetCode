using System.Reflection;
using Streetcode.BLL.DTO.Jobs;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Job
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractCreateTestJobAttribute : BeforeAfterTestAttribute
    {
        public static JobDto JobForTest { get; set; } = null!;

        public static JobCreateDto JobCreateForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            JobForTest = new JobDto
            {
                Title = "Test",
                Status = false,
                Description = "Test",
                Salary = "100",
            };

            JobCreateForTest = new JobCreateDto
            {
                Title = "Test",
                Status = false,
                Description = "Test",
                Salary = "100",
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var job = sqlDbHelper.GetExistItem<DAL.Entities.Jobs.Job>(p => p.Id == JobForTest.Id);
            if (job != null)
            {
                sqlDbHelper.DeleteItem(job);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
