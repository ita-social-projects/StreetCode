using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.AdditionalContent;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

[ApiController]
[Route("api/[controller]/[action]")]
public class SubtitleController : ControllerBase
{
    private readonly ISubtitleService _subtitleService;
    public SubtitleController(ISubtitleService subtitleService)
    {
        _subtitleService = subtitleService;
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
    public async Task<IActionResult> Create(SubtitleDTO subtitle)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(SubtitleDTO subtitle)
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