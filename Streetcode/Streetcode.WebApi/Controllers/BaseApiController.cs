using System.Resources;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Streetcode.BLL.MediatR.ResultVariations;

namespace Streetcode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private readonly IStringLocalizer _stringLocalizer;
    private IMediator? _mediator;
    public BaseApiController(IStringLocalizer<BaseApiController> stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
    }

    public BaseApiController()
    {
    }

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

                NotFound("Not Found") : Ok(result.Value);
        }

        return BadRequest(result.Reasons);
    } 
}