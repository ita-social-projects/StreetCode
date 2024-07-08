using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Art.StreetcodeArtSlide.GetAllCountByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class StreetcodeArtSlideController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetPageByStreetcodeId([FromRoute] uint streetcodeId, ushort fromSlideN, ushort amountOfSlides)
    {
        return HandleResult(await Mediator.Send(new GetArtSlidesByStreetcodeIdQuery(streetcodeId, fromSlideN, amountOfSlides)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetAllCountByStreetcodeId([FromRoute] uint streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetAllCountByStreetcodeIdQuerry(streetcodeId)));
    }
}
