
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class TermController : ControllerBase
{
    private readonly ITermService _termService;
    public TermController(ITermService termService)
    {
        _termService = termService;
    }
    [HttpPost("createTerm")]
    public void CreateTerm()
    {
        // TODO implement here
    }
    [HttpGet("getTerm")]
    public string GetTerm() 
    {
        return _termService.GetTermAsync();
    }
    [HttpPut("UpdateTerm")]
    public void UpdateTerm()
    {
        // TODO implement here
    }
    [HttpDelete("deleteTerm")]
    public void DeleteTerm()
    {
        // TODO implement here
    }

}