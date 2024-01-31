using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.AdditionalContent.Tag
{
    public class StreetcodeTagIndexSetup
    {
        public static void Setup(int streetcodeId, DAL.Entities.AdditionalContent.Tag tagId)
        {
            //var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            //var existingIndex = sqlDbHelper.GetExistItem<StreetcodeTagIndex>(
            //index => index.StreetcodeId == streetcodeForTest.Id && index.TagId == tagForTest.Id);
            //if (existingIndex == null)
            //{
            //    var streetcodeTagIndex = new StreetcodeTagIndex { TagId = tagForTest.Id, StreetcodeId = streetcodeForTest.Id };
            //    sqlDbHelper.AddNewItem(streetcodeTagIndex);
            //    sqlDbHelper.SaveChanges();
            //}
        }
    }
}
