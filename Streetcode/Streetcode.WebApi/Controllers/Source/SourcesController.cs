using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Partners;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoryById;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetCategoriesByStreetcodeId;
using Streetcode.BLL.MediatR.Sources.SourceLink.GetSubCategoriesByCategoryId;

namespace Streetcode.WebApi.Controllers.Source;

public class SourcesController : BaseApiController
{
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
    public async Task<IActionResult> Create([FromBody] PartnerDTO partner)
    {
        // TODO implement here
        return Ok();
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