using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class SubtitleController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SubtitleDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllSubtitlesQuery(GetUserRole())));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubtitleDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetSubtitleByIdQuery(id, GetUserRole())));
    }

    [HttpGet("{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SubtitleDTO))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetSubtitlesByStreetcodeIdQuery(streetcodeId, GetUserRole())));
    }
}