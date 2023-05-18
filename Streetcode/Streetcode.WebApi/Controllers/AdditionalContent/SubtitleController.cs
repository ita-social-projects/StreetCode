using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.AdditionalContent.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetAll;
using Streetcode.BLL.MediatR.AdditionalContent.Subtitle.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class SubtitleController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllSubtitlesQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetSubtitleByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetSubtitlesByStreetcodeIdQuery(streetcodeId)));
    }
}