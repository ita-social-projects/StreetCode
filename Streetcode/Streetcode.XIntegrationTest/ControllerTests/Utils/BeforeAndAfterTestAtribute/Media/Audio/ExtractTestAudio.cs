using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ServiceTests.BlobServiceTests.Utils;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Media.Audio
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    internal class ExtractTestAudio : BeforeAfterTestAttribute
    {
        public static DAL.Entities.Media.Audio? AudioForTest;
        public static StreetcodeContent StreetcodeWithAudio;

        public override void Before(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            StreetcodeWithAudio = sqlDbHelper.GetExistItem<StreetcodeContent>(x => x.AudioId != null);
            if (StreetcodeWithAudio == null)
            {
                BlobStorageFixture blobFixture = new BlobStorageFixture();
                AudioForTest = blobFixture.SeedAudio(Guid.NewGuid().ToString());
                AudioForTest.BlobName += $".{AudioForTest.MimeType?.Split('/')[1]}";
                StreetcodeContent streetcode = new StreetcodeContent()
                {
                    Index = new Random().Next(0, 1000000),
                    UpdatedAt = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    EventStartOrPersonBirthDate = DateTime.Now,
                    EventEndOrPersonDeathDate = DateTime.Now,
                    ViewCount = 1,
                    DateString = "20 травня 2023",
                    Title = "withAudio",
                    TransliterationUrl = Guid.NewGuid().ToString(),
                    Audio = AudioForTest,
                };
                sqlDbHelper.AddNewItem(streetcode);
                sqlDbHelper.SaveChanges();
                StreetcodeWithAudio = streetcode;
            }
            else
            {
                AudioForTest = sqlDbHelper.GetExistItem<DAL.Entities.Media.Audio?>(x => x.Id == StreetcodeWithAudio.AudioId);
            }
        }
    }
}
