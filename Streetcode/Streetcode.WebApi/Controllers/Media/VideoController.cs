using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Video;
using Streetcode.BLL.MediatR.Media.Video.GetAll;
using Streetcode.BLL.MediatR.Media.Video.GetById;
using Streetcode.BLL.MediatR.Media.Video.GetByStreetcodeId;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Media;

public class VideoController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VideoDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllVideosQuery(GetUserRole())));
    }

    [HttpGet("{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VideoDTO))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetVideoByStreetcodeIdQuery(streetcodeId, GetUserRole())));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VideoDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetVideoByIdQuery(id, GetUserRole())));
    }
}
