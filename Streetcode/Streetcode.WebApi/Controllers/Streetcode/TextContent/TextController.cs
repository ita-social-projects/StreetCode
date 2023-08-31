using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.DTO.Streetcode.TextContent.Text;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Streetcode.Text.GetParsed;

namespace Streetcode.WebApi.Controllers.Streetcode.TextContent;

public class TextController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllTextsQuery()));
    }

    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var isAdmin = HttpContext.User.IsInRole("Administrator");
        if (isAdmin)
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetTextByIdQuery(id)));
        }
        else
        {
            return HandleResult(await Mediator.Send(new GetTextByIdQuery(id)));
        }
    }

    [HttpGet("{streetcodeId:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        var isAdmin = HttpContext.User.IsInRole("Administrator");
        if (isAdmin)
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetTextByStreetcodeIdQuery(streetcodeId)));
        }
        else
        {
            return HandleResult(await Mediator.Send(new GetTextByStreetcodeIdQuery(streetcodeId)));
        }
    }

    [HttpPost]
    public async Task<IActionResult> UpdateParsedText([FromBody] TextPreviewDTO text)
    {
        return HandleResult(await Mediator.Send(new UpdateParsedTextForAdminPreviewCommand(text.TextContent)));
    }
}