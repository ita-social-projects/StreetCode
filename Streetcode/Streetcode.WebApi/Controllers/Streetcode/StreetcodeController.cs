using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class StreetcodeController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
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

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByIdQuery(id)));
    }

    [HttpGet("{tagId:int}")]
    public async Task<IActionResult> GetByTagId([FromRoute] int tagId)
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

    [HttpGet("{index}")]
    public async Task<IActionResult> GetByIndex([FromRoute] int index)
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StreetcodeDTO streetcode)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StreetcodeDTO streetcode)
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