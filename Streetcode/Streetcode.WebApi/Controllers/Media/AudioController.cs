using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;

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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AudioDTO audio)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] AudioDTO audio)
    {
        // TODO implement here
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // TODO implement here
        return Ok();
    }
}