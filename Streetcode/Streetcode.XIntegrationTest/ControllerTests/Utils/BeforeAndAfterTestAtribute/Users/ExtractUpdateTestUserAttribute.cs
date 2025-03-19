using System.Reflection;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Users;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ExtractUpdateTestUserAttribute : BeforeAfterTestAttribute
{
    public static User? TestUserEntity { get; private set; }

    public static UpdateUserDTO UserForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();

        var uniqueId = Guid.NewGuid().ToString();
        string testUserName = $"testuser_{uniqueId.Substring(0, 8)}".ToLower();
        string testEmail = $"test_{uniqueId.Substring(0, 8)}@example.com";

        TestUserEntity = new User
        {
            Name = "testname",
            Surname = "testsurname",
            Id = uniqueId,
            UserName = testUserName,
            NormalizedUserName = testUserName.ToUpper(),
            Email = testEmail,
            PasswordHash = GenerateTestPassword(),
        };

        sqlDbHelper.AddNewItem(TestUserEntity);
        sqlDbHelper.SaveChanges();

        UserForTest = new UpdateUserDTO
        {
            Id = "Test_User_User_qwe123456rty#",
            Name = "UpdatedName",
            Surname = "UpdatedSurname",
            AboutYourself = "Updated description",
            PhoneNumber = "+380735004490",
        };
    }

    private string GenerateTestPassword()
    {
        string guid = Guid.NewGuid().ToString();
        return $"TestPass123_{guid.Substring(0, 10)}";
    }
}