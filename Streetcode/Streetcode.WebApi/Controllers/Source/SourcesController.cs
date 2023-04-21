using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.MediatR.Sources.SourceLink.Delete;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll;

namespace Streetcode.WebApi.Controllers.Source;

public class SourcesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllNames()
    {
        return HandleResult(await Mediator.Send(new GetAllCategoryNamesQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetCategoriesByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetCategoriesByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSubCategoriesByCategoryId([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetSubCategoriesByCategoryIdQuery(id)));
    }

    [HttpPost]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> CreateCategory([FromBody] SourceLinkDTO category)
    {
        // TODO implement here
        return HandleResult(await Mediator.Send(new CreateCategoryCommand(category)));
    }

    [HttpPut("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] SourceLinkDTO category)
    {
        return HandleResult(await Mediator.Send(new UpdateCategoryCommand(category)));
    }

    [HttpDelete("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteCategoryCommand(id)));
    }
}