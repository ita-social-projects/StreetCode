using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class StreetcodeArtController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StreetcodeArtDto>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StreetcodeArtSlideDto>))]
    public async Task<IActionResult> GetPageByStreetcodeId([FromRoute] uint streetcodeId, ushort page, ushort pageSize)
    {
        return HandleResult(await Mediator.Send(new GetArtSlidesByStreetcodeIdQuery(streetcodeId, page, pageSize)));
    }
}
