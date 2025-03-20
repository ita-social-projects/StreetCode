using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Media.Images;

[ValidateStreetcodeExistence]
public class StreetcodeArtSlideController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<StreetcodeArtSlideDTO>))]
    public async Task<IActionResult> GetPageByStreetcodeId([FromRoute] uint streetcodeId, ushort fromSlideN, ushort amountOfSlides)
    {
        return HandleResult(await Mediator.Send(new GetArtSlidesByStreetcodeIdQuery(streetcodeId, fromSlideN, amountOfSlides, GetUserRole())));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> GetAllCountByStreetcodeId([FromRoute] uint streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetAllCountByStreetcodeIdQuerry(streetcodeId, GetUserRole())));
    }
}
