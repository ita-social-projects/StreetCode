
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Timeline;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimelineController : ControllerBase 
{
    private readonly ITimelineService _timelineService;
    public TimelineController(ITimelineService timelineService)
    {
        _timelineService = timelineService;
    }
    [HttpGet("getTimelineById")]
    public void GetTimelineById() 
    {
        // TODO implement here
    }
    [HttpGet("getTimeLineItems")]
    public void GetTimelineItems()
    {
        // TODO implement here
    }
    [HttpPost("createTimeline")]
    public void CreateTimeline()
    {
        // TODO implement here
    }
    [HttpGet("getTimeline")]
    public void GetTimeline()
    {
        // TODO implement here
    }
    [HttpPut("updateTimeline")]
    public void UpdateTimeline() 
    {
        // TODO implement here
    }
    [HttpDelete("deleteTimeline")]
    public void DeleteTimeline() 
    {
        // TODO implement here
    }

}