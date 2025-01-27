using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent.Coordinates.Types;
using Streetcode.BLL.MediatR.AdditionalContent.Coordinate.GetByStreetcodeId;
using Streetcode.WebApi.Filters;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class CoordinateController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    [TypeFilter(typeof(ValidateStreetcodeExistenceAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StreetcodeCoordinateDTO>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetCoordinatesByStreetcodeIdQuery(streetcodeId)));
    }
}