using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Images;
using Streetcode.BLL.MediatR.Media.Image.GetAll;
using Streetcode.BLL.MediatR.Media.Image.GetBaseImage;
using Streetcode.BLL.MediatR.Media.Image.GetById;
using Streetcode.BLL.MediatR.Media.Image.GetByStreetcodeId;
using Streetcode.BLL.MediatR.Media.Image.Create;
using Streetcode.BLL.MediatR.Media.Image.Delete;
using Streetcode.BLL.MediatR.Media.Image.Update;
using Streetcode.DAL.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class ImageController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllImagesQuery()));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ImageDTO>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetImageByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
       return HandleResult(await Mediator.Send(new GetImageByIdQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ImageDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Create([FromBody] ImageFileBaseCreateDTO image)
    {
        return HandleResult(await Mediator.Send(new CreateImageCommand(image)));
    }

    [HttpPut]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ImageDTO))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Update([FromBody] ImageFileBaseUpdateDTO image)
    {
        return HandleResult(await Mediator.Send(new UpdateImageCommand(image)));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteImageCommand(id)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MemoryStream))]
    public async Task<IActionResult> GetBaseImage([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetBaseImageQuery(id)));
    }
}
