using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.AdditionalContent;
using Streetcode.BLL.DTO.AdditionalContent.Tag;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.Create;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetAll;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetById;
using Streetcode.BLL.MediatR.AdditionalContent.Tag.GetByStreetcodeId;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers.AdditionalContent;

public class TagController : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllTagsQuery()));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        return HandleResult(await Mediator.Send(new GetTagByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    public async Task<IActionResult> GetByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetTagByStreetcodeIdQuery(streetcodeId)));
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetTagByTitle([FromRoute] string title)
    {
        return HandleResult(await Mediator.Send(new GetTagByTitleQuery(title)));
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Create([FromBody] CreateTagDTO tagTitle)
    {
        return HandleResult(await Mediator.Send(new CreateTagQuery(tagTitle)));
    }
}
