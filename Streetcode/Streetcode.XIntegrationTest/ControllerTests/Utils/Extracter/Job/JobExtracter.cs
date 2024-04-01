
namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job
{
    public class JobExtracter
    {
        public static DAL.Entities.Jobs.Job Extract(int jobId)
        {
            DAL.Entities.Jobs.Job testJob = TestDataProvider.GetTestData<DAL.Entities.Jobs.Job>();

            testJob.Id = jobId;

            return BaseExtracter.Extract<DAL.Entities.Jobs.Job>(testJob, job => job.Id == jobId);
        }

        public static void Remove(DAL.Entities.Jobs.Job entity)
        {
            BaseExtracter.RemoveByPredicate<DAL.Entities.Jobs.Job>(tag => tag.Id == entity.Id);
        }
    }
}
