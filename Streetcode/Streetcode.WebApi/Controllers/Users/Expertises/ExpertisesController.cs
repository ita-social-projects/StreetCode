using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Expertises.GetAll;

namespace Streetcode.WebApi.Controllers.Users.Expertises;

public class ExpertisesController : BaseApiController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        return HandleResult(await Mediator.Send(new GetAllExpertisesQuery()));
    }
}