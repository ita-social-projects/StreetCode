using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag
{
    public class StreetcodeTagIndexSetup
    {
        public static void Setup(StreetcodeContent streetcodeForTest, DAL.Entities.AdditionalContent.Tag tagForTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var existingIndex = sqlDbHelper.GetExistItem<StreetcodeTagIndex>(
            index => index.StreetcodeId == streetcodeForTest.Id && index.TagId == tagForTest.Id);
            if (existingIndex == null)
            {
                var streetcodeTagIndex = new StreetcodeTagIndex { TagId = tagForTest.Id, StreetcodeId = streetcodeForTest.Id };
                sqlDbHelper.AddNewItem(streetcodeTagIndex);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
