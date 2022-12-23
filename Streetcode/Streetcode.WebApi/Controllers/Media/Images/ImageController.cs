using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.Interfaces.Media.Images;

namespace Streetcode.WebApi.Controllers.Media.Images;

[ApiController]
[Route("api/[controller]/[action]")]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;
    public ImageController(IImageService imageService)
    {
        _imageService = imageService;
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
    public async Task<IActionResult> Create(ImageDTO image)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Update(ImageDTO image)
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
