using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;

namespace Streetcode.WebApi.Controllers.Streetcode.TextContent;

[ApiController]
[Route("api/[controller]/[action]")]
public class FactController : ControllerBase
{
    private readonly IFactService _factService;

    public FactController(IFactService factService)
    {
        _factService = factService;
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

    [HttpGet("{streetcodeId}")]
    public async Task<IActionResult> GetByStreetcodeId(int streetcodeId)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(FactDTO fact)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(FactDTO fact)
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