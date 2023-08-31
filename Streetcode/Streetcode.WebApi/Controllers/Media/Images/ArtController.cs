using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Image.GetById;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class ArtController : BaseApiController
{
    [HttpGet]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllArtsQuery()));
    }

    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var isAdmin = HttpContext.User.IsInRole("Administrator");
        if (isAdmin)
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetArtByIdQuery(id)));
        }
        else
        {
            return HandleResult(await Mediator.Send(new GetArtByIdQuery(id)));
        }
    }

    [HttpGet("{streetcodeId:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetAllByStreetcodeId([FromRoute] int streetcodeId)
    {
        var isAdmin = HttpContext.User.IsInRole("Administrator");
        if (isAdmin)
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetArtsByStreetcodeIdQuery(streetcodeId)));
        }
        else
        {
            return HandleResult(await Mediator.Send(new GetArtsByStreetcodeIdQuery(streetcodeId)));
        }
    }
}
