using Streetcode.DAL.Entities.Media.Images;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.MediaExtracter.Image;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;

public static class FactExtracter
{
    public static Fact Extract(int factId, string? title = null)
    {
        var streetcodeContentId = UniqueNumberGenerator.GenerateInt();
        var imageId = UniqueNumberGenerator.GenerateInt();

        var testFact = TestDataProvider.GetTestData<Fact>();
        var testStreetcodeContent = StreetcodeContentExtracter.Extract(
            streetcodeContentId,
            streetcodeContentId,
            Guid.NewGuid().ToString());
        var testImage = ImageExtracter.Extract(imageId);

        testFact.Id = factId;
        testFact.ImageId = testImage.Id;
        testFact.Image = testImage;
        testFact.StreetcodeId = testStreetcodeContent.Id;
        testFact.Streetcode = testStreetcodeContent;
        testFact.Index = factId;

        if (title is not null)
        {
            testFact.Title = title;
        }

        return BaseExtracter.Extract(testFact, fact => fact.Id == factId);
    }

    public static void Remove(Fact factEntity)
    {
        BaseExtracter.RemoveById<Fact>(factEntity.Id);
        BaseExtracter.RemoveById<StreetcodeContent>(factEntity.StreetcodeId);
        BaseExtracter.RemoveById<Image>(factEntity.ImageId!.Value);
    }
}
