using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Delete;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.DeleteSoft;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.UpdateStatus;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Requests.Streetcode;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class StreetcodeController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllStreetcodesRequest request)
    {
        return HandleResult(await Mediator.Send(new GetAllStreetcodesQuery(
            request.Page, request.Amount, request.Title, request.Sort, request.Filter)));
    }

    [HttpGet("{index:int}")]
    public async Task<IActionResult> ExistWithIndex([FromRoute] int index)
    {
        return HandleResult(await Mediator.Send(new StreetcodeWithIndexExistQuery(index)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByIdQuery(id)));
    }

    [HttpGet("{index}")]
    public async Task<IActionResult> GetByIndex([FromRoute] int index)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByIndexQuery(index)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StreetcodeDTO streetcode)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StreetcodeDTO streetcode)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPatch("{id:int}/{status}")]
    public async Task<IActionResult> PatchStage(
        [FromRoute] int id,
        [FromRoute] StreetcodeStatus status)
    {
        return HandleResult(await Mediator.Send(new UpdateStatusStreetcodeByIdCommand(id, status)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> SoftDelete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteSoftStreetcodeCommand(id)));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteStreetcodeCommand(id)));
    }
}