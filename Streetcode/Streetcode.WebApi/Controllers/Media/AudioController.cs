using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Audio;
using Streetcode.BLL.MediatR.Media.Audio.Create;
using Streetcode.BLL.MediatR.Media.Audio.Delete;
using Streetcode.BLL.MediatR.Media.Audio.GetAll;
using Streetcode.BLL.MediatR.Media.Audio.GetBaseAudio;
using Streetcode.BLL.MediatR.Media.Audio.GetById;
using Streetcode.BLL.MediatR.Media.Audio.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Audio.Update;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Media;

public class AudioController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AudioDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllAudiosQuery(GetUserRole())));
    }

    [HttpGet("{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDTO))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetAudioByStreetcodeIdQuery(streetcodeId, GetUserRole())));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetAudioByIdQuery(id, GetUserRole())));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemoryStream))]
    public async Task<IActionResult> GetBaseAudio([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetBaseAudioQuery(id, GetUserRole())));
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AudioDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] AudioFileBaseCreateDTO audio)
    {
        return HandleResult(await Mediator.Send(new CreateAudioCommand(audio)));
    }

    [HttpPut]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update([FromBody] AudioFileBaseUpdateDTO audio)
    {
        return HandleResult(await Mediator.Send(new UpdateAudioCommand(audio)));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteAudioCommand(id)));
    }
}