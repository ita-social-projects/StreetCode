using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class StreetcodeArtSlideController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetPageByStreetcodeId([FromRoute] uint streetcodeId, ushort fromSlideN, ushort amountOfSlides)
    {
        return HandleResult(await Mediator.Send(new GetArtSlidesByStreetcodeIdQuery(streetcodeId, fromSlideN, amountOfSlides)));
    }
}
