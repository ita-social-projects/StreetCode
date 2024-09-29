using Streetcode.DAL.Entities.Media.Images;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.Partner
{
    public static class PartnerExtracter
    {
        public static DAL.Entities.Partners.Partner Extract(int partnerId)
        {
            DAL.Entities.Partners.Partner testPartner = TestDataProvider.GetTestData<DAL.Entities.Partners.Partner>();
            Image testImage = ImageExtracter.Extract(partnerId);
            testPartner.Id = partnerId;
            testPartner.LogoId = testImage.Id;

            return BaseExtracter.Extract<DAL.Entities.Partners.Partner>(testPartner, partner => partner.Id == partnerId);
        }

        public static void Remove(DAL.Entities.Partners.Partner entity)
        {
            BaseExtracter.RemoveByPredicate<DAL.Entities.Partners.Partner>(partner => partner.Id == entity.Id);
            BaseExtracter.RemoveById<Image>(entity.Id);
        }
    }
}
