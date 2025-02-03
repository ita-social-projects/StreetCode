using System.Reflection;
using Streetcode.BLL.DTO.Authentication.Register;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Authentication
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ExtractRegisterRequestAttribute : BeforeAfterTestAttribute
    {
        public static RegisterRequestDto RegisterRequest { get; set; } = null!;

        public override void Before(MethodInfo methodUnderTest)
        {
            RegisterRequest = new RegisterRequestDto()
            {
                Email = "test@register.com",
                Name = "Test",
                Surname = "Test",
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
