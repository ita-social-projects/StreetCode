using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.MediatR.Sources.SourceLink.Delete;
using Streetcode.DAL.Enums;
using Streetcode.WebApi.Attributes;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId;

namespace Streetcode.WebApi.Controllers.Source;

public class SourcesController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllNames()
    {
        return HandleResult(await Mediator.Send(new GetAllCategoryNamesQuery()));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        return HandleResult(await Mediator.Send(new GetAllCategoriesQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
    }

    [HttpGet("{categoryId:int}&{streetcodeId:int}")]
    public async Task<IActionResult> GetCategoryContentByStreetcodeId([FromRoute] int streetcodeId, [FromRoute] int categoryId)
    {
        return HandleResult(await Mediator.Send(new GetCategoryContentByStreetcodeIdQuery(streetcodeId, categoryId)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetCategoriesByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetCategoriesByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] SourceLinkCategoryDTO category)
    {
        return HandleResult(await Mediator.Send(new CreateCategoryCommand(category)));
    }

    [HttpPut("{id:int}")]
    [AuthorizeRoles(UserRole.MainAdministrator, UserRole.Administrator)]
    public async Task<IActionResult> UpdateCategory([FromBody] SourceLinkCategoryDTO category)
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
