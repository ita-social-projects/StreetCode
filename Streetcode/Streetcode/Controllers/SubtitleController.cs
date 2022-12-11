
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;


namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class SubtitleController : ControllerBase
{
    private readonly ISubtitleService _subtitleService;
    public SubtitleController(ISubtitleService subtitleService) 
    {
        _subtitleService= subtitleService;
    }
    [HttpPost("createSubtitle")]
    public void CreateSubtitle() 
    {
        // TODO implement here
    }
    [HttpGet("getSubtitle")]
    public void GetSubtitle()
    {
        // TODO implement here
    }
    [HttpPut("updateSubtitle")]
    public void UpdateSubtitle()
    {
        // TODO implement here
    }
    [HttpDelete("deleteSubtitle")]
    public void DeleteSubtitle() 
    {
        // TODO implement here
    }
    [HttpGet("getSubtitleById")]
    public void GetSubtitleById() 
    {
        // TODO implement here
    }
    [HttpGet("getSubtitlesByStreetcode")]
    public string GetSubtitlesByStreetcode()
    {
        return _subtitleService.GetSubtitlesByStreetcode();
        // TODO implement here
    }

}