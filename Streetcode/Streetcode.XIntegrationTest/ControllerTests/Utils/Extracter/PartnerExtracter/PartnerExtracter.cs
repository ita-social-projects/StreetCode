using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Partners;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.PartnerExtracter
{
    public static class PartnerExtracter
    {
        public static Partner Extract(int partnerId)
        {
            Partner testPartner = TestDataProvider.GetTestData<Partner>();
            Image testImage = ImageExtracter.Extract(partnerId);
            testPartner.Id = partnerId;
            testPartner.LogoId = testImage.Id;

            return BaseExtracter.Extract<Partner>(testPartner, partner => partner.Id == partnerId);
        }

        public static void Remove(Partner entity)
        {
            BaseExtracter.RemoveByPredicate<Partner>(partner => partner.Id == entity.Id);
            BaseExtracter.RemoveById<Image>(entity.Id);
        }
    }
}
