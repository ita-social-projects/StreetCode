using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using Streetcode.DAL.Entities.Streetcode.TextContent;
using Streetcode.DAL.Persistence;

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

    [HttpGet("getFactByStreetcodeId")]
    public void GetFactsByStreetcodeId()
    {
        // TODO implement here
    }

    [HttpPost("createFact")]
    public void CreateFact()
    {
        // TODO implement here
    }

    [HttpGet("getAllFacts")]
    public void GetAllFacts()
    {
        // TODO implement here
    }

    [HttpPut("updateFact")]
    public void UpdateFact()
    {
        // TODO implement here
    }

    [HttpDelete("deleteFact")]
    public void DeleteFact()
    {
        // TODO implement here
    }
}