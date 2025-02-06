using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToUpdateLocalizer : BaseMockStringLocalizer<FailedToUpdateSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                0, new List<string>
                {
                    "FailedToUpdateStatusOfStreetcode",
                }
            },
        };
    }
}