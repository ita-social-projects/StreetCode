﻿using Microsoft.AspNetCore.Authorization;
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

namespace Streetcode.WebApi.Controllers.Media;

public class AudioController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AudioDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllAudiosQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDTO))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetAudioByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AudioDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetAudioByIdQuery(id)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemoryStream))]
    public async Task<IActionResult> GetBaseAudio([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetBaseAudioQuery(id)));
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