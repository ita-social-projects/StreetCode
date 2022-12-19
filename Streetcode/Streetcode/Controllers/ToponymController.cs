using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Toponyms;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToponymController : ControllerBase
{
    private readonly IToponymService _toponymService;
    public ToponymController(IToponymService toponymService)
    {
        _toponymService = toponymService;
    }

    [HttpPost("createToponym")]
    public void CreateToponym()
    {
        // TODO implement here
    }

    [HttpGet("getToponym")]
    public void GetToponym()
    {
        // TODO implement here
    }

    [HttpPut("updateToponym")]
    public void UpdateToponym()
    {
        // TODO implement here
    }

    [HttpDelete("deleteToponym")]
    public void DeleteToponym()
    {
        // TODO implement here
    }

    [HttpGet("getToponymByName")]
    public void GetToponymByName()
    {
        // TODO implement here
    }

    [HttpGet("getStreetcodesByToponym")]
    public void GetStreetcodesByToponym()
    {
        // TODO implement here
    }
}