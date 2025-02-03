using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.DTO.Sources;
using Streetcode.BLL.MediatR.Sources.SourceLink.Create;
using Streetcode.BLL.MediatR.Sources.SourceLink.Update;
using Streetcode.BLL.MediatR.Sources.SourceLink.Delete;
using Streetcode.DAL.Enums;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetAll;
using Streetcode.BLL.MediatR.Sources.SourceLinkCategory.GetCategoryContentByStreetcodeId;
using Microsoft.AspNetCore.Authorization;
using Streetcode.WebApi.Attributes;

namespace Streetcode.WebApi.Controllers.Source;

public class SourcesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryWithNameDto>))]
    public async Task<IActionResult> GetAllNames()
    {
        return HandleResult(await Mediator.Send(new GetAllCategoryNamesQuery()));
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetAllCategoriesResponseDto))]
    public async Task<IActionResult> GetAllCategories([FromQuery] ushort? page, [FromQuery] ushort? pageSize)
    {
        return HandleResult(await Mediator.Send(new GetAllCategoriesQuery(page, pageSize)));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SourceLinkCategoryDto))]
    public async Task<IActionResult> GetCategoryById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetCategoryByIdQuery(id)));
    }

    [HttpGet("{categoryId:int}&{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StreetcodeCategoryContentDto))]
    public async Task<IActionResult> GetCategoryContentByStreetcodeId([FromRoute] int streetcodeId, [FromRoute] int categoryId)
    {
        return HandleResult(await Mediator.Send(new GetCategoryContentByStreetcodeIdQuery(streetcodeId, categoryId)));
    }

    [HttpGet("{streetcodeId:int}")]
    [ValidateStreetcodeExistence]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SourceLinkCategoryDto>))]
    public async Task<IActionResult> GetCategoriesByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetCategoriesByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateSourceLinkCategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateCategory([FromBody] SourceLinkCategoryCreateDto category)
    {
        return HandleResult(await Mediator.Send(new CreateCategoryCommand(category)));
    }

    [HttpPut]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateSourceLinkCategoryDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]

    public async Task<IActionResult> UpdateCategory([FromBody] UpdateSourceLinkCategoryDto category)
    {
        return HandleResult(await Mediator.Send(new UpdateCategoryCommand(category)));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCategory([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new DeleteCategoryCommand(id)));
    }
}
