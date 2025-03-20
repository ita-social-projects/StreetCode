using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Media.Image
{
    public static class ArtExtracter
    {
        public static Art Extract(int artId, int strId)
        {
            DAL.Entities.Media.Images.Image testImage = ImageExtracter.Extract(artId);
            Art testArt = TestDataProvider.GetTestData<Art>();
            testArt.Id = artId;
            testArt.ImageId = testImage.Id;
            testArt.StreetcodeId = strId;

            return BaseExtracter.Extract<Art>(testArt, art => art.Id == artId);
        }

        public static void Remove(Art entity)
        {
            BaseExtracter.RemoveById<Art>(entity.Id);
            BaseExtracter.RemoveById<DAL.Entities.Media.Images.Image>(entity.ImageId);
        }

        public static void AddStreetcodeArt(int streetcodeId, int artId)
        {
            StreetcodeArt streetcodeArt = new StreetcodeArt()
            {
                StreetcodeId = streetcodeId,
                ArtId = artId,
            };
            BaseExtracter.Extract<StreetcodeArt>(
                streetcodeArt,
                strArt => strArt.ArtId == artId && strArt.StreetcodeId == streetcodeId,
                hasIdentity: false);
        }

        public static void AddStreetcodeArtWithStreetcodeArtSlide(int streetcodeId, int artId, int streetcodeArtSlideId)
        {
            StreetcodeArt streetcodeArt = new StreetcodeArt()
            {
                StreetcodeId = streetcodeId,
                ArtId = artId,
                StreetcodeArtSlideId = streetcodeArtSlideId,
            };
            BaseExtracter.Extract<StreetcodeArt>(
                streetcodeArt,
                strArt =>
                    strArt.ArtId == artId
                    && strArt.StreetcodeId == streetcodeId
                    && strArt.StreetcodeArtSlideId == streetcodeArtSlideId,
                hasIdentity: false);
        }
    }
}
