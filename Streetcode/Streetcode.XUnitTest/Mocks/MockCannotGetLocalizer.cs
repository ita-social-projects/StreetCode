using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockCannotGetLocalizer : BaseMockStringLocalizer<CannotGetSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "CannotGetWordsByTermId",
                }
            },
        };

        return groupedErrors;
    }
}