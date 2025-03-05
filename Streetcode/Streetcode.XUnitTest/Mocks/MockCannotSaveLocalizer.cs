using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockCannotSaveLocalizer : BaseMockStringLocalizer<CannotSaveSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "CannotSaveChangesInTheDatabaseAfterRelatedWordCreation",
                }
            },
        };

        return groupedErrors;
    }
}