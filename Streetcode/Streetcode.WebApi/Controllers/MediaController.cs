using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.Interfaces.Media;

namespace Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _mediaService;
    public MediaController(IMediaService mediaService)
    {
        _mediaService = mediaService;
    }

    [HttpGet("getPicture")]
    public void GetPicture()
    {
        // TODO implement here
    }

    [HttpPost("uploadPicture")]
    public void UploadPicture()
    {
        // TODO implement here
    }

    [HttpDelete("deletePicture")]
    public void DeletePicture()
    {
        // TODO implement here
    }

    [HttpGet("getVideo")]
    public void GetVideo()
    {
        // TODO implement here
    }

    [HttpPost("uploadVideo")]
    public void UploadVideo()
    {
        // TODO implement here
    }

    [HttpDelete("deleteVideo")]
    public void DeleteVideo()
    {
        // TODO implement here
    }

    [HttpGet("GetAudio")]
    public void GetAudio()
    {
        // TODO implement here
    }

    [HttpPost("UploadAudio")]
    public void UploadAudio()
    {
        // TODO implement here
    }

    [HttpDelete("DeleteAudio")]
    public void DeleteAudioAsync()
    {
        // TODO implement here
    }
}