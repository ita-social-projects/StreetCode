using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.MediatR.Media.StreetcodeArt.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class StreetcodeArtController : BaseApiController
{
    [HttpGet("{streetcodeId:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        var isAdmin = HttpContext.User.IsInRole("MainAdministrator");
        if (isAdmin)
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId)));
        }
        else
        {
            return HandleResult(await Mediator.Send(new GetStreetcodeArtByStreetcodeIdQuery(streetcodeId)));
        }
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetPageByStreetcodeId([FromRoute] uint streetcodeId, ushort page, ushort pageSize)
    {
        return HandleResult(await Mediator.Send(new GetArtSlidesByStreetcodeIdQuery(streetcodeId, page, pageSize)));
    }
}
