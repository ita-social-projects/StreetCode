using Microsoft.AspNetCore.Mvc;
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

    [HttpGet("{tagId:int}")]
    public async Task<IActionResult> GetByTagId([FromRoute] int tagId)
    {
        return HandleResult(await Mediator.Send(new GetRelatedFiguresByTagIdQuery(tagId)));
    }
}