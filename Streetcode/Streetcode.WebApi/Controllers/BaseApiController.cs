using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.ResultVariations;

namespace Streetcode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>()!;

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if(result is NullResult<T>)
            {
                return Ok(result.Value);
            }

            return (result.Value is null) ?
                NotFound("Found result matching null") : Ok(result.Value);
        }

        return BadRequest(result.Reasons);
    }
}