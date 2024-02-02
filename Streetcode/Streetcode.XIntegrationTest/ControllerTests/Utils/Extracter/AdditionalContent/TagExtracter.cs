using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent
{
    public class TagExtracter
    {
        public static Tag Extract(int tagId)
        {
            Tag testTag = TestDataProvider.GetTestData<Tag>();

            testTag.Id = tagId;

            return BaseExtracter.Extract<Tag>(testTag, tag => tag.Id == tagId);
        }

        public static void Remove(Tag entity)
        {
            BaseExtracter.RemoveByPredicate<Tag>(tag => tag.Id == entity.Id);
        }

        public static void AddStreetcodeTagIndex(int streetcodeId, int tagId)
        {
            StreetcodeTagIndex streetcodeTagIndex = new StreetcodeTagIndex()
            {
                StreetcodeId = streetcodeId,
                TagId = tagId,
            };
            BaseExtracter.Extract<StreetcodeTagIndex>(
                streetcodeTagIndex,
                strTagInd => strTagInd.TagId == tagId && strTagInd.StreetcodeId == streetcodeId,
                hasIdentity: false);
        }
    }
}
