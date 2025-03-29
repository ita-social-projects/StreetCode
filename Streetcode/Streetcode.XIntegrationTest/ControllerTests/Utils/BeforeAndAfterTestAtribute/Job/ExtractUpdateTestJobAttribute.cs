using System.Reflection;
using Streetcode.BLL.DTO.Jobs;
using DALJob = Streetcode.DAL.Entities.Jobs.Job;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Job
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractUpdateJobAttribute : BeforeAfterTestAttribute
    {
        private DALJob _job = null!; 

        public static JobUpdateDto JobForTest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            int uniqueId = UniqueNumberGenerator.GenerateInt();
            _job = JobExtracter.Extract(uniqueId);  

            JobForTest = new JobUpdateDto
            {
                Id = _job.Id,
                Title = _job.Title,
                Status = _job.Status,
                Description = _job.Description,
                Salary = _job.Salary,
            };

            JobForTest.Title = "Updated Test Job Title";
            JobForTest.Description = "Updated test description";
            JobForTest.Salary = 50000.ToString();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();

            var job = sqlDbHelper.GetExistItem<DALJob>(t => t.Id == JobForTest.Id); 
            if (job != null)
            {
                sqlDbHelper.DeleteItem(job);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}