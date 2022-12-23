using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Streetcode;

namespace Streetcode.WebApi.Controllers.Streetcode;

[ApiController]
[Route("api/[controller]")]
public class StreetcodeController : ControllerBase
{
    private readonly IStreetcodeService _streetcodeService;
    public StreetcodeController(IStreetcodeService streetcodeService)
    {
        _streetcodeService = streetcodeService;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByTagId(int id)
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(int name)
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{index}")]
    public async Task<IActionResult> GetByIndex(int index)
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

    [HttpGet]
    public async Task<IActionResult> GetEvents()
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetPersons()
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StreetcodeDTO fact)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(StreetcodeDTO fact)
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