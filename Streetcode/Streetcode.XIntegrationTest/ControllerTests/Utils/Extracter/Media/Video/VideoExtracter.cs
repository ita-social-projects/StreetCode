using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Video
{
    public class VideoExtracter
    {
        public static DAL.Entities.Media.Video Extract(int videoId)
        {
            DAL.Entities.Media.Video testVideo = TestDataProvider
                .GetTestData<DAL.Entities.Media.Video>();
            StreetcodeContent testStreetcodeContent = StreetcodeContentExtracter.Extract(
                videoId,
                videoId,
                Guid.NewGuid().ToString());
            testVideo.Id = videoId;
            testVideo.StreetcodeId = testStreetcodeContent.Id;

            return BaseExtracter.Extract<DAL.Entities.Media.Video>(testVideo, video => video.Id == videoId);
        }

        public static void Remove(DAL.Entities.Media.Video entity)
        {
            BaseExtracter.RemoveByPredicate<DAL.Entities.Media.Video>(video => video.Id == entity.Id);
            BaseExtracter.RemoveById<StreetcodeContent>(entity.Id);
        }
    }
}
