using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class ToponymController : BaseApiController
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

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ToponymDTO toponym)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ToponymDTO toponym)
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