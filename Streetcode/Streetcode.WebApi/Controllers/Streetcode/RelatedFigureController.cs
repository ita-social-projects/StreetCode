using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode.RelatedFigure;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Create;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.Delete;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetAllPublished;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Streetcode.RelatedFigure.GetByTagId;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class RelatedFigureController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RelatedFigureDTO>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetRelatedFigureByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{tagId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RelatedFigureDTO>))]
    public async Task<IActionResult> GetByTagId([FromRoute] int tagId)
    {
        return HandleResult(await Mediator.Send(new GetRelatedFiguresByTagIdQuery(tagId)));
    }

    [HttpPost("{ObserverId:int}&{TargetId:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromRoute] int ObserverId, int TargetId)
    {
        return HandleResult(await Mediator.Send(new CreateRelatedFigureCommand(ObserverId, TargetId)));
    }

    [HttpDelete("{ObserverId:int}&{TargetId:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int ObserverId, int TargetId)
    {
        return HandleResult(await Mediator.Send(new DeleteRelatedFigureCommand(ObserverId, TargetId)));
    }
}
