using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    public static class StreetcodeIndexFetch
    {
        public static int GetStreetcodeByIndex(int index)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var existingStreetcode = sqlDbHelper.GetExistItem<StreetcodeContent>(p => p.Index == index) !;
            var id = existingStreetcode.Id;
            return id;
        }
    }
}
