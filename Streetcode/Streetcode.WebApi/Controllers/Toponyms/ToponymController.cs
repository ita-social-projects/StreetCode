using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Toponyms.GetAll;
using Streetcode.BLL.MediatR.Toponyms.GetById;
using Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class ToponymController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllToponymsResponseDTO))]
    public async Task<IActionResult> GetAll([FromQuery] GetAllToponymsRequestDTO request)
    {
        return HandleResult(await Mediator.Send(new GetAllToponymsQuery(request, GetUserRole())));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ToponymDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetToponymByIdQuery(id, GetUserRole())));
    }

    [HttpGet("{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ToponymDTO>))]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetToponymsByStreetcodeIdQuery(streetcodeId, GetUserRole())));
    }
}