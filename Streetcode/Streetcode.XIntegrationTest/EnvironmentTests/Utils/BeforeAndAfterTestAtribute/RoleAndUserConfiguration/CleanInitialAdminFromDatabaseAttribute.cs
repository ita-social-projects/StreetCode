using System.Reflection;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Streetcode.XIntegrationTest.ControllerTests.Utils;
using Xunit.Sdk;
using static Streetcode.WebApi.Utils.Constants.UserDatabaseSeedingConstants;

namespace Streetcode.XIntegrationTest.EnvironmentTests.Utils.BeforeAndAfterTestAtribute.RoleAndUserConfiguration
{
    public class CleanInitialAdminFromDatabaseAttribute : BeforeAfterTestAttribute
    {
        private readonly SqlDbHelper sqlDbHelper;

        public CleanInitialAdminFromDatabaseAttribute()
        {
            this.sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
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
            User? initialAdminFromDb = this.sqlDbHelper
                .GetExistItem<User>(user => user.Email == AdminEmail && user.UserName == AdminUsername);

            if (initialAdminFromDb is not null)
            {
                this.sqlDbHelper.DeleteItem(initialAdminFromDb);
                this.sqlDbHelper.SaveChanges();
            }
        }
    }
}
