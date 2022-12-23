using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.Interfaces.AdditionalContent;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

[ApiController]
[Route("api/[controller]/[action]")]
public class TagController : ControllerBase
{
    private readonly ITagService _tagService;
    public TagController(ITagService tagService)
    {
        _tagService = tagService;
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

    [HttpGet("{title}")]
    public async Task<IActionResult> GetTagByTitle(string title)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TagDTO tag)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(TagDTO tag)
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