using MediatR;
using Microsoft.AspNetCore.Mvc;
using Streetcode.BLL.MediatR.Instagram.GetAll;
using Streetcode.BLL.MediatR.Payment;
using Streetcode.BLL.MediatR.Team.Position.GetAll;
using Streetcode.DAL.Entities.Instagram;

namespace Streetcode.WebApi.Controllers.Instagram;
    public class InstagramController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await Mediator.Send(new GetAllPostsQuery()));
        }
    }