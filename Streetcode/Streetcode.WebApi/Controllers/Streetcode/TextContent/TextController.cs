using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;

namespace Streetcode.WebApi.Controllers.Streetcode.TextContent;

[ApiController]
[Route("api/[controller]/[action]")]
public class TextController : ControllerBase
{
    private readonly ITextService _textService;
    public TextController(ITextService textService)
    {
        _textService = textService;
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

    [HttpPost]
    public async Task<IActionResult> Create(TextDTO fact)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(TextDTO fact)
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