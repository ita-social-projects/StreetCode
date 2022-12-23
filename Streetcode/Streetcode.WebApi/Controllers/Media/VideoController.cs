using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Media;
using Streetcode.BLL.Interfaces.Media.Images;

namespace Streetcode.WebApi.Controllers.Media;
[ApiController]
[Route("api/[controller]/[action]")]
public class VideoController : ControllerBase
{
    private readonly IVideoService _videoService;
    public VideoController(IVideoService videoService)
    {
        _videoService = videoService;
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
    public async Task<IActionResult> Create(VideoDTO video)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(VideoDTO video)
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
