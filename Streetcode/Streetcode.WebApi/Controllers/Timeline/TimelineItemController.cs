using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Timeline;
using Streetcode.BLL.Interfaces.Timeline;

namespace Streetcode.WebApi.Controllers.Timeline;

[ApiController]
[Route("api/[controller]/[action]")]
public class TimelineItemController : ControllerBase
{
    private readonly ITimelineItemService _timelineService;
    public TimelineItemController(ITimelineItemService timelineService)
    {
        _timelineService = timelineService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TimelineItemDTO timeline)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(TimelineItemDTO timeline)
    {
        // TODO implement here
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        // TODO implement here
        return Ok();
    }
}