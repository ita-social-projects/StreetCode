using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class TagController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        // TODO implement here
        return Ok();
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetTagByTitle([FromRoute] string title)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TagDTO tag)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TagDTO tag)
    {
        // TODO implement here
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // TODO implement here
        return Ok();
    }
}