// using FluentResults;

using System.Resources;
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
    private readonly ResourceManager _resourceManager;

    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>()!;

    public BaseApiController(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager;
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if(result is NullResult<T>)
            {
                return Ok(result.Value);
            }

            return (result.Value is null) ?
                NotFound(_resourceManager.GetString("NotFound")) : Ok(result.Value);
        }

        foreach (var item in result.Reasons)
        {
            if (item.Message.Contains(_resourceManager.GetString("Cannot find an audio")))
            {
                return Ok();
            }
        }

        return BadRequest(result.Reasons);
    }
}