using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.News
{
    internal class NewsExtracter
    {
        public static DAL.Entities.News.News Extract(int newsId)
        {
            DAL.Entities.News.News testNews = TestDataProvider.GetTestData<DAL.Entities.News.News>();
            Image testImage = ImageExtracter.Extract(newsId);
            testNews.Id = newsId;
            testNews.Image = testImage;
            testNews.ImageId = testImage.Id;

            return BaseExtracter.Extract<DAL.Entities.News.News>(testNews, news => news.Id == newsId);
        }

        public static void Remove(DAL.Entities.News.News entity)
        {
            BaseExtracter.RemoveByPredicate<DAL.Entities.News.News>(news => news.Id == entity.Id);
            BaseExtracter.RemoveById<Image>(entity.ImageId);
        }
    }
}
