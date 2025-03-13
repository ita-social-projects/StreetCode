using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToUpdateLocalizer : BaseMockStringLocalizer<FailedToUpdateSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "FailedToUpdateFact",
                    "FailedToUpdateTerm",
                    "FailedToUpdateStatusOfStreetcode",
                    "FailedToChangeStatusOfStreetcodeToDeleted",
                    "FailedToUpdateEvent",
                }
            },
        };

        return groupedErrors;
    }
}