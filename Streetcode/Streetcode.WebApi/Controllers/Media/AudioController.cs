using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.MediatR.Media.Audio.Create;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Media;

public class AudioController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllAudiosQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        var isAdmin = HttpContext.User.IsInRole("MainAdministrator");
        if (!isAdmin)
        {
            return HandleResult(await Mediator.Send(new GetAudioByStreetcodeIdQuery(streetcodeId)));
        }
        else
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetAudioByStreetcodeIdQuery(streetcodeId)));
        }
    }

    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 600, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var isAdmin = HttpContext.User.IsInRole("MainAdministrator");
        if (!isAdmin)
        {
            return HandleResult(await Mediator.Send(new GetAudioByIdQuery(id)));
        }
        else
        {
            Response.Headers[HeaderNames.CacheControl] = "no-store, no-cache";
            return HandleResult(await Mediator.Send(new GetAudioByIdQuery(id)));
        }
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBaseAudio([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetBaseAudioQuery(id)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AudioFileBaseCreateDTO audio)
    {
        return HandleResult(await Mediator.Send(new CreateAudioCommand(audio)));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] AudioFileBaseUpdateDTO audio)
    {
        return HandleResult(await Mediator.Send(new UpdateAudioCommand(audio)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAudioCommand(id)));
    }
}