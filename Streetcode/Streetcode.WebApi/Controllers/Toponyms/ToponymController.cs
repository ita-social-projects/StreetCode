using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Toponyms.GetAll;
using Streetcode.BLL.MediatR.Toponyms.GetById;
using Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class ToponymController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllToponymsResponseDto))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllToponymsRequestDto request)
    {
        return HandleResult(await Mediator.Send(new GetAllToponymsQuery(request)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToponymDto))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetToponymByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ToponymDto>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetToponymsByStreetcodeIdQuery(streetcodeId)));
    }
}