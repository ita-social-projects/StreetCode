using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class CoordinateController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetCoordinatesByStreetcodeIdQuery(streetcodeId)));
    }
}