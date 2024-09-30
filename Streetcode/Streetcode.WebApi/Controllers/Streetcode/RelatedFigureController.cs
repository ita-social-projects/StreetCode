using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId;
using Streetcode.DAL.Enums;

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

    [HttpPost("{observerId:int}&{targetId:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromRoute] int observerId, int targetId)
    {
        return HandleResult(await Mediator.Send(new CreateRelatedFigureCommand(observerId, targetId)));
    }

    [HttpDelete("{observerId:int}&{targetId:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int observerId, int targetId)
    {
        return HandleResult(await Mediator.Send(new DeleteRelatedFigureCommand(observerId, targetId)));
    }
}
