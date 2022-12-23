using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Media;
using Streetcode.BLL.Interfaces.Media.Images;

namespace Streetcode.WebApi.Controllers.Media;
[ApiController]
[Route("api/[controller]/[action]")]
public class AudioController : ControllerBase
{
    private readonly IAudioService _audioService;
    public AudioController(IAudioService audioService)
    {
        _audioService = audioService;
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
    public async Task<IActionResult> Create(AudioDTO audio)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(AudioDTO audio)
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