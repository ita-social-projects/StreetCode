using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.DTO.Shared;
using Streetcode.BLL.MediatR.ResultVariations;
using Streetcode.DAL.Enums;

namespace Streetcode.WebApi.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

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

    protected UserRole? GetUserRole()
    {
        var user = HttpContext?.User;
        if (user == null)
        {
            return null;
        }

        foreach (UserRole role in Enum.GetValues(typeof(UserRole)))
        {
            if (user.IsInRole(role.ToString()))
            {
                return role;
            }
        }

        return null;
    }
}