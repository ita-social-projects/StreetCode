using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Media.Images;

namespace Streetcode.WebApi.Controllers.Media.Images;

[ApiController]
[Route("api/[controller]/[action]")]
public class ArtController : ControllerBase
{
    private readonly IArtService _artService;
    public ArtController(IArtService artService)
    {
        _artService = artService;
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
    public async Task<IActionResult> Create(ArtDTO art)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(ArtDTO art)
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
