using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Partners;

public class PartnersController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllPartnersQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetPartnerByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetPartnersByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartnerDTO partner)
    {
        return HandleResult(await Mediator.Send(new CreatePartnerQuery(partner)));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PartnerDTO partner)
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