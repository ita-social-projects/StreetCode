using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Toponyms;
using Streetcode.BLL.MediatR.Toponyms.GetAll;
using Streetcode.BLL.MediatR.Toponyms.GetById;
using Streetcode.BLL.MediatR.Toponyms.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Toponyms;

public class ToponymController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllToponymsRequestDTO request)
    {
        return HandleResult(await Mediator.Send(new GetAllToponymsQuery(request)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetToponymByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetToponymsByStreetcodeIdQuery(streetcodeId)));
    }
}