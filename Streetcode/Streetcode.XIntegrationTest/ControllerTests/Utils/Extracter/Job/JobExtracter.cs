
namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Job
{
    public class JobExtracter
    {
        public static DAL.Entities.Jobs.Job Extract(int jobId)
        {
            DAL.Entities.Jobs.Job testJob = TestDataProvider.GetTestData<DAL.Entities.Jobs.Job>();

            testJob.Id = jobId;
            var extracter = BaseExtracter.Extract<DAL.Entities.Jobs.Job>(testJob, job => job.Id == jobId);
            return extracter;
        }

        public static DAL.Entities.Jobs.Job ExtractByTitle(string title)
        {
            DAL.Entities.Jobs.Job testJob = TestDataProvider.GetTestData<DAL.Entities.Jobs.Job>();

            testJob.Title = title;
            var extracter = BaseExtracter.Extract<DAL.Entities.Jobs.Job>(testJob, job => job.Title.Equals(title));
            return extracter;
        }

        public static void Remove(DAL.Entities.Jobs.Job entity)
        {
            BaseExtracter.RemoveByPredicate<DAL.Entities.Jobs.Job>(tag => tag.Id == entity.Id);
        }
    }
}
