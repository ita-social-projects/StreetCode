using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Audio.CreateNewAudio;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Audio.Update;

namespace Streetcode.WebApi.Controllers.Media;

public class AudioController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllAudiosQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetAudioByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetAudioByIdQuery(id)));
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