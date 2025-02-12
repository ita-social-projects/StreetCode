using Streetcode.DAL.Entities.Streetcode.TextContent;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.TextContent;

public static class TermExtracter
{
    public static Term Extract(int termId, string? title = null)
    {
        var testTerm = TestDataProvider.GetTestData<Term>();

        testTerm.Id = termId;

        if (title is not null)
        {
            testTerm.Title = title;
        }

        return BaseExtracter.Extract(testTerm, term => term.Id == termId);
    }

    public static void Remove(Term termEntity)
    {
        BaseExtracter.RemoveById<Term>(termEntity.Id);
    }
}
