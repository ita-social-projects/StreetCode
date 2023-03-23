using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Streetcode;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAll;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetById;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByIndex;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.WithIndexExist;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class StreetcodeController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllStreetcodesQuery()));
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

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        // TODO implement here
        return Ok();
    }
}