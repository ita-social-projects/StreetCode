using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Streetcode
{
    public class StreetcodeIndexFetch
    {
        public static int GetStreetcodeByIndex(int index)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var existingStreetcode = sqlDbHelper.GetExistItem<StreetcodeContent>(p => p.Index == index);
            var id = existingStreetcode.Id;
            return id;
        }
    }
}
