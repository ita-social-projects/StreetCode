using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockCannotFindLocalizer : BaseMockStringLocalizer<CannotFindSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        return new Dictionary<int, List<string>>
        {
            {
                0, new List<string>
                {
                    "CannotFindAnyPropertyWithThisName",
                    "CannotFindStreetcodeById",
                    "CannotFindRecordWithQrId",
                }
            },
            {
                1, new List<string>()
                {
                    "CannotFindAnyStreetcodeWithCorrespondingId",
                }
            },
        };
    }
}