using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToDeleteLocalizer : BaseMockStringLocalizer<FailedToDeleteSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "FailedToDeleteFact",
                    "FailedToDeleteRelatedTerm",
                    "FailedToDeleteTerm",
                    "FailedToDeleteText",
                    "FailedToDeleteStreetcode",
                    "FailedToDeleteRelation",
                    "FailedToDeleteEvent",
                }
            },
        };

        return groupedErrors;
    }
}