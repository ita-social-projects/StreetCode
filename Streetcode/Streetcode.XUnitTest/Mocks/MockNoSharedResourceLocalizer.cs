using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockNoSharedResourceLocalizer : BaseMockStringLocalizer<NoSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                1, new List<string>
                {
                    "NoExistingStreetcodeWithId",
                }
            },
        };
    }
}