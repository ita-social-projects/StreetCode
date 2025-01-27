using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Media.Art;
using Streetcode.BLL.MediatR.Media.Art.GetAll;
using Streetcode.BLL.MediatR.Media.Art.GetById;
using Streetcode.BLL.MediatR.Media.Art.GetByStreetcodeId;
using Streetcode.WebApi.Filters;

namespace Streetcode.WebApi.Controllers.Media.Images;

public class ArtController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArtDTO>))]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllArtsQuery()));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArtDTO))]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
            return HandleResult(await Mediator.Send(new GetArtByIdQuery(id)));
    }

    [HttpGet("{streetcodeId:int}")]
    [TypeFilter(typeof(ValidateStreetcodeExistenceAttribute))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArtDTO>))]
    public async Task<IActionResult> GetArtsByStreetcodeId([FromRoute] int streetcodeId)
    {
        return HandleResult(await Mediator.Send(new GetArtsByStreetcodeIdQuery(streetcodeId)));
    }
}
