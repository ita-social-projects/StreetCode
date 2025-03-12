using Streetcode.BLL.SharedResource;

namespace Streetcode.XUnitTest.Mocks;

public class MockCannotFindLocalizer : BaseMockStringLocalizer<CannotFindSharedResource>
{
    protected override Dictionary<int, List<string>> DefineGroupedErrors()
    {
        var groupedErrors = new Dictionary<int, List<string>>()
        {
            {
                0, new List<string>()
                {
                    "CannotFindAnyFact",
                    "CannotFindAnyTerm",
                    "CannotFindAnyText",
                    "CannotFindStreetcodeInFavourites",
                    "CannotFindAnyPropertyWithThisName",
                    "CannotFindStreetcodeById",
                    "CannotFindRecordWithQrId",
                    "CannotFindAnyEvents",
                }
            },
            {
                1, new List<string>()
                {
                    "CannotFindFactWithCorrespondingCategoryId",
                    "CannotFindRelatedTermWithCorrespondingId",
                    "CannotFindAnyTermWithCorrespondingId",
                    "CannotFindTextWithCorrespondingCategoryId",
                    "CannotFindAnyTextWithCorrespondingId",
                    "CannotFindTransactionLinkByStreetcodeIdBecause",
                    "CannotFindHistoricalContextWithCorrespondingId",
                    "CannotFindAnyStreetcodeWithCorrespondingId",
                    "CannotFindEventWithCorrespondingId",
                }
            },
            {
                2, new List<string>()
                {
                    "CannotFindRelationBetweenStreetcodesWithCorrespondingIds",
                }
            },
        };

        return groupedErrors;
    }
}