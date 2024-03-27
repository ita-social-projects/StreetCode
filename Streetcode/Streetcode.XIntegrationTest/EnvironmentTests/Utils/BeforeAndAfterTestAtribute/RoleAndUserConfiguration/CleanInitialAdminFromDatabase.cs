using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using System.Reflection;
using Xunit.Sdk;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.XIntegrationTest.EnvironmentTests.Utils.BeforeAndAfterTestAtribute.RoleAndUserConfiguration
{
    public class CleanInitialAdminFromDatabase : BeforeAfterTestAttribute
    {
        private readonly SqlDbHelper _sqlDbHelper;

        public CleanInitialAdminFromDatabase()
        {
            this._sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        }

        public override void Before(MethodInfo methodUnderTest)
        {
            this.DeleteInitialAdminIfExists();
        }

        public override void After(MethodInfo methodUnderTest)
        {
            this.DeleteInitialAdminIfExists();
        }

        private void DeleteInitialAdminIfExists()
        {
            User initialAdminFromDb = this._sqlDbHelper
                .GetExistItem<User>(user => user.Email == AdminEmail && user.UserName == AdminUsername);

            if (initialAdminFromDb is not null)
            {
                this._sqlDbHelper.DeleteItem<User>(initialAdminFromDb);
                this._sqlDbHelper.SaveChanges();
            }
        }
    }
}
