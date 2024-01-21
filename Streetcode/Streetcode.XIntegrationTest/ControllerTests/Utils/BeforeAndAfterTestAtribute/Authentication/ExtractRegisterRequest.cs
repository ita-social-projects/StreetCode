using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using System.Reflection;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractRegisterRequest : BeforeAfterTestAttribute
    {
        public static RegisterRequestDTO RegisterRequest;

        public override void Before(MethodInfo methodUnderTest)
        {
            RegisterRequest = new RegisterRequestDTO()
            {
                Email = "test@register.com",
                UserName = "Test_Register",
                Name = "Test",
                Surname = "Test",
                PhoneNumber = "+111-111-11-11",
                Password = "qwQWE45$vlm*asB3545",
                PasswordConfirmation = "qwQWE45$vlm*asB3545",
            };
        }

        public override void After(MethodInfo methodUnderTest)
        {
            var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
            var userFromDb = sqlDbHelper.GetExistItem<User>(user => user.Email == RegisterRequest.Email);
            if (userFromDb != null)
            {
                sqlDbHelper.DeleteItem(userFromDb);
                sqlDbHelper.SaveChanges();
            }
        }
    }
}
