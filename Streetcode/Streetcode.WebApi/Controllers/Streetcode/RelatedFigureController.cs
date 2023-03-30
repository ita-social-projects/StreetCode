using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Fact.Delete;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class RelatedFigureController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpPost("{ObserverId:int}&{TargetId:int}")]
    public async Task<IActionResult> Create([FromRoute] int ObserverId, int TargetId)
    {
        return HandleResult(await Mediator.Send(new CreateRelatedFigureCommand(ObserverId, TargetId)));
    }

    [HttpDelete("{ObserverId:int}&{TargetId:int}")]
    public async Task<IActionResult> Delete([FromRoute] int ObserverId, int TargetId)
    {
        return HandleResult(await Mediator.Send(new DeleteRelatedFigureCommand(ObserverId, TargetId)));
    }
}