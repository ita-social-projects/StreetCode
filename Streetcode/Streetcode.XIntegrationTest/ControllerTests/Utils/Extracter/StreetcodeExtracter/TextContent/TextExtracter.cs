using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;

public static class TextExtracter
{
    public static Text Extract(int textId)
    {
        var streetcodeContentId = UniqueNumberGenerator.GenerateInt();

        var testText = TestDataProvider.GetTestData<Text>();
        var testStreetcodeContent = StreetcodeContentExtracter.Extract(
            streetcodeContentId,
            streetcodeContentId,
            Guid.NewGuid().ToString());

        testText.Id = textId;
        testText.StreetcodeId = testStreetcodeContent.Id;
        testText.Streetcode = testStreetcodeContent;

        return BaseExtracter.Extract(testText, text => text.Id == textId);
    }

    public static void Remove(Text textEntity)
    {
        BaseExtracter.RemoveById<Text>(textEntity.Id);
        BaseExtracter.RemoveById<StreetcodeContent>(textEntity.StreetcodeId);
    }
}
