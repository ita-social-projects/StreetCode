using Streetcode.DAL.Enums;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image
{
    public static class StreetcodeArtSlideExtracter
    {
        public static DAL.Entities.Streetcode.StreetcodeArtSlide Extract(int id)
        {
            DAL.Entities.Streetcode.StreetcodeArtSlide testStreetcodeArtSlide = TestDataProvider.GetTestData<DAL.Entities.Streetcode.StreetcodeArtSlide>();
            testStreetcodeArtSlide.Id = id;

            return BaseExtracter.Extract(
                testStreetcodeArtSlide,
                streetcodeArtSlide => streetcodeArtSlide.Id == id);
        }

        public static void Remove(DAL.Entities.Streetcode.StreetcodeArtSlide entity)
        {
            BaseExtracter.RemoveById<DAL.Entities.Streetcode.StreetcodeArtSlide>(entity.Id);
        }

        public static void AddStreetcodeArtSlide(int streetcodeId, int template, int index)
        {
            DAL.Entities.Streetcode.StreetcodeArtSlide streetcodeArtSlide = new DAL.Entities.Streetcode.StreetcodeArtSlide()
            {
                StreetcodeId = streetcodeId,
                Template = (StreetcodeArtSlideTemplate)template,
                Index = index,
            };
            BaseExtracter.Extract(
                streetcodeArtSlide,
                strArtSlide => strArtSlide.StreetcodeId == streetcodeId,
                hasIdentity: false);
        }
    }
}
