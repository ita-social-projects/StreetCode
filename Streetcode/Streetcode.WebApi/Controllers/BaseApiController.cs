﻿using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Shared;
using Streetcode.BLL.MediatR.ResultVariations;

namespace Streetcode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    public BaseApiController()
    {
    }

    protected IMediator Mediator => _mediator ??=
        HttpContext.RequestServices.GetService<IMediator>() !;
    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            if (result is NullResult<T>)
            {
                return Ok(result.Value);
            }

            return (result.Value is null) ?
                NotFound("Not Found") : Ok(result.Value);
        }

        if (result.HasError(error => error.Message == "Unauthorized"))
        {
            return Unauthorized();
        }

        return BadRequest(result.Errors.Select(x => new ErrorDto() { Message = x.Message }));
    }
}