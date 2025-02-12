using Streetcode.BLL.MediatR.Streetcode.RelatedTerm.Create;

namespace Streetcode.XUnitTest.Mocks;

public class MockCreateRelatedTermLocalizer : BaseMockStringLocalizer<CreateRelatedTermHandler>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "WordWithThisDefinitionAlreadyExists",
                }
            },
        };

        return groupedErrors;
    }
}