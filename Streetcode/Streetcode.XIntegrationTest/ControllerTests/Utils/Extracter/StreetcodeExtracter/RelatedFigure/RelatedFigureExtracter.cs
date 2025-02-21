using Streetcode.DAL.Entities.AdditionalContent;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.Base;
using Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.AdditionalContent;
using RelatedFigureEntity = Streetcode.DAL.Entities.Streetcode.RelatedFigure;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.RelatedFigure;

public static class RelatedFigureExtracter
{
    public static (StreetcodeContent, StreetcodeContent, RelatedFigureEntity, Tag) ExtractTestData()
    {
        var observerId = UniqueNumberGenerator.GenerateInt();
        var targetId = UniqueNumberGenerator.GenerateInt();

        var testStreetcodeContent1 = StreetcodeContentExtracter.Extract(
            observerId,
            observerId,
            Guid.NewGuid().ToString());

        var testStreetcodeContent2 = StreetcodeContentExtracter.Extract(
            targetId,
            targetId,
            Guid.NewGuid().ToString());

        var testRelatedFigure = Extract(testStreetcodeContent1.Id, testStreetcodeContent2.Id);

        var testTag = TagExtracter.Extract(targetId, Guid.NewGuid().ToString());

        return (testStreetcodeContent1, testStreetcodeContent2, testRelatedFigure, testTag);
    }

    public static RelatedFigureEntity Extract(int observerId, int targetId)
    {
        var testRelatedFigure = new RelatedFigureEntity
        {
            ObserverId = observerId,
            TargetId = targetId,
        };

        var extractor = BaseExtracter.Extract(testRelatedFigure, figure =>
            figure.ObserverId == observerId && figure.TargetId == targetId);
        return extractor;
    }

    public static void Remove(RelatedFigureEntity entity)
    {
        BaseExtracter.RemoveByPredicate<RelatedFigureEntity>(
            figure => figure.ObserverId == entity.ObserverId && figure.TargetId == entity.TargetId);
    }
}