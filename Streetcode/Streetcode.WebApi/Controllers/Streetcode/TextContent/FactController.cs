using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode.TextContent;
using Streetcode.BLL.Interfaces.Streetcode.TextContent;
using Streetcode.BLL.MediatR.Streetcode.Fact.Queries;

namespace Streetcode.WebApi.Controllers.Streetcode.TextContent;

[ApiController]
[Route("api/[controller]/[action]")]
public class FactController : ControllerBase
{
    private readonly IMediator _mediator;

    public FactController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var facts = await _mediator.Send(new GetAllFactsQuery());

        // TODO implement here
        return Ok(facts);
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