using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockUserSharedResourceLocalizer : BaseMockStringLocalizer<UserSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                0, new List<string>
                {
                    "UserWithSuchUsernameNotExists",
                    "UserWithSuchUsernameExists",
                    "UserWithSuchEmailNotFound",
                    "UserWithSuchEmailExists",
                    "UserNotFound",
                    "UserNameOrTokenIsEmpty",
                    "UserManagerError",
                    "IncorrectPassword",
                    "EmailNotMatch",
                }
            },
        };
    }
}