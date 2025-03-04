using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockCannotCreateLocalizer : BaseMockStringLocalizer<CannotCreateSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "CannotCreateNewRelatedWordForTerm",
                    "CannotCreateDTOsForRelatedWords",
                    "CannotCreateTerm",
                }
            },
        };

        return groupedErrors;
    }
}