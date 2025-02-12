using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.XIntegrationTest.Base;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;

public static class RelatedTermExtracter
{
    public static RelatedTerm Extract(int relatedTermId, string? word = null)
    {
        var termId = UniqueNumberGenerator.GenerateInt();

        var testRelatedTerm = TestDataProvider.GetTestData<RelatedTerm>();
        var testTerm = TermExtracter.Extract(termId);

        testRelatedTerm.Id = relatedTermId;
        testRelatedTerm.TermId = testTerm.Id;
        testRelatedTerm.Term = testTerm;

        if (word is not null)
        {
            testRelatedTerm.Word = word;
        }

        return BaseExtracter.Extract(testRelatedTerm, relatedTerm => relatedTerm.Id == relatedTermId);
    }

    public static void Remove(RelatedTerm relatedTermEntity)
    {
        BaseExtracter.RemoveById<RelatedTerm>(relatedTermEntity.Id);
        BaseExtracter.RemoveById<Term>(relatedTermEntity.TermId);
    }
}
