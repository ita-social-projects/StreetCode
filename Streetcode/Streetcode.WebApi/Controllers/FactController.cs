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
    private readonly StreetcodeDbContext _context;
    private readonly IFactService _factService;
    private readonly ILoggerService<FactController> _loggerService;

    /*public FactController(IFactService factService, ILoggerService<FactController> loggerService)
    {
        _factService = factService;
        _loggerService = loggerService;
    }
    */

    public FactController(StreetcodeDbContext context)
    {
        _context = context;
    }

    [HttpGet("getFactById")]
    public void GetFactById()
    {
        // TODO implement here
    }

    [HttpGet("getFactByStreetcodeId")]
    public string GetFactsByStreetcodeId()
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

    [HttpGet("getAllFacts")]
    public async Task<IEnumerable<Fact>> GetAllFacts()
    {
        // TODO implement here
        var facts = await _context.Facts.ToListAsync();
        return facts;
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