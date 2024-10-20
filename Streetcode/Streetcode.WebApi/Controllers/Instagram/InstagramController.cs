using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Instagram.GetAll;

namespace Streetcode.WebApi.Controllers.Instagram;
    public class InstagramController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<InstagramPost>))]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllPostsQuery()));
        }
    }