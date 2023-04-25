using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Partners.Create;
using Streetcode.BLL.MediatR.Partners.GetAll;
using Streetcode.BLL.MediatR.Partners.GetAllPartnerShort;
using Streetcode.BLL.MediatR.Partners.GetById;
using Streetcode.BLL.MediatR.Partners.GetByStreetcodeId;
using Streetcode.DAL.Entities.Partners;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Partners;

public class PartnersController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllPartnersQuery()));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShort()
    {
        return HandleResult(await Mediator.Send(new GetAllPartnersShortQuery()));
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
    /*[AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]*/
    public async Task<IActionResult> Create([FromBody] CreatePartnerDTO partner)
    {
        return HandleResult(await Mediator.Send(new CreatePartnerQuery(partner)));
    }

    [HttpPut]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> Update([FromBody] CreatePartnerDTO partner)
    {
        return HandleResult(await Mediator.Send(new BLL.MediatR.Partners.Update.UpdatePartnerQuery(partner)));
    }

    [HttpDelete("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new BLL.MediatR.Partners.Delete.DeletePartnerQuery(id)));
    }
}
