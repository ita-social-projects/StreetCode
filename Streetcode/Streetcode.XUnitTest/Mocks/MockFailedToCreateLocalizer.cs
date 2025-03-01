using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockFailedToCreateLocalizer : BaseMockStringLocalizer<FailedToCreateSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "FailedToCreateFact",
                    "FailedToCreateTerm",
                    "FailedToMapCreatedTerm",
                    "FailedToCreateStreetcode",
                    "TheStreetcodesAreAlreadyLinked",
                    "FailedToCreateRelation",
                }
            },
        };

        return groupedErrors;
    }
}