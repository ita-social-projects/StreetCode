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
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetByTransliterationUrl;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllShort;
using Streetcode.WebApi.Attributes;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetAllCatalog;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetCount;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.Create;
using Streetcode.BLL.DTO.Streetcode.Create;
using Streetcode.BLL.MediatR.Streetcode.Streetcode.GetUrlByQrId;

namespace Streetcode.WebApi.Controllers.Streetcode;

public class StreetcodeController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllStreetcodesRequestDTO request)
    {
        return HandleResult(await Mediator.Send(new GetAllStreetcodesQuery(request)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllShort()
    {
        return HandleResult(await Mediator.Send(new GetAllStreetcodesShortQuery()));
    }

    [HttpGet("{index:int}")]
    public async Task<IActionResult> ExistWithIndex([FromRoute] int index)
    {
        return HandleResult(await Mediator.Send(new StreetcodeWithIndexExistQuery(index)));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCatalog([FromQuery] int page, [FromQuery] int count)
    {
        return HandleResult(await Mediator.Send(new GetAllStreetcodesCatalogQuery(page, count)));
    }

    [HttpGet]
    public async Task<IActionResult> GetCount()
    {
        return HandleResult(await Mediator.Send(new GetStreetcodesCountQuery()));
    }

    [HttpGet("{url}")]
    public async Task<IActionResult> GetByTransliterationUrl([FromRoute] string url)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByTransliterationUrlQuery(url)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByIdQuery(id)));
    }

    [HttpGet("{qrid:int}")]
    public async Task<IActionResult> GetByQrId([FromRoute] int qrid)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeUrlByQrIdQuery(qrid)));
    }

    [HttpGet("{index}")]
    public async Task<IActionResult> GetByIndex([FromRoute] int index)
    {
        return HandleResult(await Mediator.Send(new GetStreetcodeByIndexQuery(index)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] StreetcodeCreateDTO streetcode)
    {
        return HandleResult(await Mediator.Send(new CreateStreetcodeCommand(streetcode)));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] StreetcodeDTO streetcode)
    {
        // TODO implement here
        return Ok();
    }

    [HttpPatch("{id:int}/{status}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> PatchStage(
        [FromRoute] int id,
        [FromRoute] StreetcodeStatus status)
    {
        return HandleResult(await Mediator.Send(new UpdateStatusStreetcodeByIdCommand(id, status)));
    }

    [HttpDelete("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> SoftDelete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteSoftStreetcodeCommand(id)));
    }

    [HttpDelete("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteStreetcodeCommand(id)));
    }
}
