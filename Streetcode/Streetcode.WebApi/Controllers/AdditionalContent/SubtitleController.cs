using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent.Subtitles;
using Streetcode.BLL.MediatR.Streetcode.Text.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Text.GetById;
using Streetcode.BLL.MediatR.Streetcode.Text.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Subtitle.GetAll;
using Streetcode.BLL.MediatR.Subtitle.GetById;
using Streetcode.BLL.MediatR.Subtitle.GetByStreetcodeId;

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
        return HandleResult(await Mediator.Send(new GetSubtitleByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SubtitleDTO subtitle)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SubtitleDTO subtitle)
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