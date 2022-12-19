using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class FactController : ControllerBase
{
    private readonly IFactService _factService;
    private readonly ILoggerService<FactController> _loggerService;

    public FactController(IFactService factService, ILoggerService<FactController> loggerService)
    {
        _factService = factService;
        _loggerService = loggerService;
    }

    [HttpGet("getFactById")]
    public void GetFactById()
    {
        // TODO implement here
    }

    [HttpGet("getFactByStreetcode")]
    public string GetFactsByStreetcode()
    {
        // TODO implement here
        _loggerService.LogError("Error????????");
        return _factService.GetFactsByStreetcodeAsync();
    }

    [HttpPost("createFact")]
    public void CreateFact()
    {
        // TODO implement here
    }

    [HttpGet("getFact")]
    public void GetFact()
    {
        // TODO implement here
    }

    [HttpPut("updateFact")]
    public void UpdateFact()
    {
        // TODO implement here
    }

    [HttpDelete("deleteFact")]
    public void DeleteFactById()
    {
        // TODO implement here
    }
}