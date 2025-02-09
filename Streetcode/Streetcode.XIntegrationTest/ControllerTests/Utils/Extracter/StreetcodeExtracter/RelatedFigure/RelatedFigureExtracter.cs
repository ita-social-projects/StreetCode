using Streetcode.DAL.Entities.Streetcode;
using Streetcode.XIntegrationTest.ControllerTests.BaseController;

namespace Streetcode.XIntegrationTest.ControllerTests.Utils.Extracter.StreetcodeExtracter.RelatedFigure;

public static class RelatedFigureExtracter
{
    public static DAL.Entities.Streetcode.RelatedFigure Extract(int observerId, int targetId)
    {
        DAL.Entities.Streetcode.RelatedFigure testRelatedFigure = new DAL.Entities.Streetcode.RelatedFigure
        {
            ObserverId = observerId,
            TargetId = targetId,
        };

        var extracter = BaseExtracter.Extract(testRelatedFigure, figure => figure.ObserverId == observerId && figure.TargetId == targetId);
        return extracter;
    }

    public static void Remove(DAL.Entities.Streetcode.RelatedFigure entity)
    {
        BaseExtracter.RemoveByPredicate<DAL.Entities.Streetcode.RelatedFigure>(figure => figure.ObserverId == entity.ObserverId && figure.TargetId == entity.TargetId);
    }
}