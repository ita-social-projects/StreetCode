using System.Reflection;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;
using Xunit.Sdk;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.BeforeAndAfterTestAtribute.Users;

[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public class ExtractUpdateTestUserAttribute : BeforeAfterTestAttribute
{
    private static User? _testUserEntity;

    public static UpdateUserDTO UserForTest { get; private set; } = null!;

    public override void Before(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();

        var uniqueId = Guid.NewGuid().ToString();
        string testUserName = $"testuser_{uniqueId.Substring(0, 8)}".ToLower();
        string testEmail = $"test_{uniqueId.Substring(0, 8)}@example.com";

        _testUserEntity = new User
        {
            Name = "testname",
            Surname = "testsurname",
            Id = uniqueId,
            UserName = testUserName,
            NormalizedUserName = testUserName.ToUpper(),
            Email = testEmail,
            PasswordHash = GenerateTestPassword(),
        };

        sqlDbHelper.AddNewItem(_testUserEntity);
        sqlDbHelper.SaveChanges();

        UserForTest = new UpdateUserDTO
        {
            Name = "UpdatedName",
            Surname = "UpdatedSurname",
            UserName = $"Updated_{testUserName}",
            AboutYourself = "Updated description",
            PhoneNumber = "+380735004490",
            Email = testEmail,
        };
    }

    public override void After(MethodInfo methodUnderTest)
    {
        var sqlDbHelper = BaseControllerTests.GetSqlDbHelper();
        var user = sqlDbHelper.GetExistItem<User>(u => u.Email == UserForTest.Email);
        if (user != null)
        {
            sqlDbHelper.DeleteItem(user);
            sqlDbHelper.SaveChanges();
        }
    }

    private static string GenerateTestPassword()
    {
        var guid = Guid.NewGuid().ToString();
        return $"TestPass123_{guid.Substring(0, 10)}";
    }
}