using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.Interfaces.Toponyms;

namespace Streetcode.WebApi.Controllers.Toponyms;

[ApiController]
[Route("api/[controller]/[action]")]
public class ToponymController : ControllerBase
{
    private readonly IToponymService _toponymService;
    public ToponymController(IToponymService toponymService)
    {
        _toponymService = toponymService;
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

    [HttpGet("{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ToponymDTO toponym)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(ToponymDTO toponym)
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