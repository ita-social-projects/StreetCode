using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Instagram.GetAll;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.DAL.Entities.Instagram;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Streetcode.WebApi.Controllers.Instagram
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstagramController : BaseApiController
    {
        [HttpGet]
        [Route("/GetPosts")]
        public async Task<IActionResult> GetAllAsync()
        {
            return HandleResult(await Mediator.Send(new GetAllPostsQuery()));
        }
    }
}
